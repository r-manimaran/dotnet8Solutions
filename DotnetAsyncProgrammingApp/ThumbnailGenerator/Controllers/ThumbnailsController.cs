using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using ThumbnailGenerator.Services;
using System.IO;
using System.Threading.Channels;
using ThumbnailGenerator.Models;
using System.Collections.Concurrent;

namespace ThumbnailGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThumbnailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly LinkGenerator _linkGenerator;
        private readonly ImageService _imageService;
        private readonly ILogger<ThumbnailsController> _logger;
        private readonly Channel<ThumbnailGeneratorJob> _channel;
        private readonly ConcurrentDictionary<string, ThumbnailGenerationStatus> _statusDictionary;
        public ThumbnailsController(IConfiguration configuration,
                                    LinkGenerator linkGenerator,
                                    ImageService imageService,
                                    ILogger<ThumbnailsController> logger,
                                    Channel<ThumbnailGeneratorJob> channel,
                                    ConcurrentDictionary<string,ThumbnailGenerationStatus> statusDictionary)
        {
            _configuration = configuration;
            _linkGenerator = linkGenerator;
            _imageService = imageService;
            _logger = logger;
            _channel = channel;
            _statusDictionary = statusDictionary;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile? file)
        {
            if (file == null) 
            {
                return BadRequest("No file uploaded.");
            }

            if (!_imageService.IsValidImage(file)) 
            {
                return BadRequest("Invalid image file. Only JPG, PNG and GIF file formats are allowed.");
            }

            var id = Guid.NewGuid().ToString();
            string _uploadDirctory = _configuration["UploadDirectory"] ?? "uploads";

            var folderPath = Path.Combine(_uploadDirctory, "images",id);
            var fileName =$"{id}{Path.GetExtension(file.FileName)}";

            var originalFilePath = await _imageService.SaveOriginalImageAsync(file, folderPath, fileName);

            //Introduce Channels - create a job
            var job = new ThumbnailGeneratorJob(id, originalFilePath, folderPath);
            await _channel.Writer.WriteAsync(job);

            _statusDictionary[id] = ThumbnailGenerationStatus.Queued;
            // code Moved to Background job
            /*
            await _imageService.GenerateThumbnailsAsync(originalFilePath, folderPath, id);

            var thumbnailLinks = ImageService.ThumbnailWidths.ToDictionary(
                width => $"w{width}",
                width => GetFullyQualifiedUrl(nameof(GetImage), new { id = id, width }));
            
            thumbnailLinks.Add("original",GetFullyQualifiedUrl(nameof(GetImage),new { id = id }));
            */
            //return Ok(new {id=id, links = thumbnailLinks});

            var statusUrl = GetFullyQualifiedUrl(nameof(GetStatus), new { id });
            return Accepted(statusUrl,new { id, status =ThumbnailGenerationStatus.Queued});

        }
        [HttpGet("{id}/status")]
        public IActionResult GetStatus(string id) 
        {
            if (!_statusDictionary.TryGetValue(id, out var status))
            {
                return NotFound();
            }
            var response = new {id,status, links = new Dictionary<string,string>() };

            if (status == ThumbnailGenerationStatus.Completed) 
            {
                var thunbnailLinks = ImageService.ThumbnailWidths.ToDictionary(
                                    width => $"w{width}",
                                    width => GetFullyQualifiedUrl(nameof(GetImage), new { id, width }));
                thunbnailLinks.Add("original", GetFullyQualifiedUrl(nameof(GetImage), new { id }));

                response = response with { links = thunbnailLinks };

            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetImage(string id, int? width = null)
        {
            string _uploadDirctory = _configuration["UploadDirectory"] ?? "uploads";
            var folderPath = Path.Combine(_uploadDirctory, "images", id);
            if (!Directory.Exists(folderPath))
            {
                return NotFound();

            }
            string fileName;
            if (width is null)
            {
                fileName = Directory.GetFiles(folderPath, $"{id}.*").FirstOrDefault() ?? string.Empty;

            }
            else
            {
                fileName = Directory.GetFiles(folderPath, $"{id}_w{width}.*").FirstOrDefault() ?? string.Empty;

            }

            if (string.IsNullOrEmpty(fileName) || !System.IO.File.Exists(fileName))
            {
                return NotFound();
            }
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            return File(fileStream, "image/jpeg");
        }

        private string GetFullyQualifiedUrl(string actionName, object values)
        {
            return _linkGenerator.GetUriByAction(
                HttpContext,
                action: actionName,
                controller: "Thumbnails",
                values: values)
                ?? throw new InvalidOperationException("Failed to generate URL.");
        }
        
    }
}
