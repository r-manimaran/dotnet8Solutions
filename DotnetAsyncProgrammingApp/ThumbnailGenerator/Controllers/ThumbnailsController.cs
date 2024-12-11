using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using ThumbnailGenerator.Services;
using System.IO;

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
        public ThumbnailsController(IConfiguration configuration,
                                    LinkGenerator linkGenerator,
                                    ImageService imageService,
                                    ILogger<ThumbnailsController> logger)
        {
            _configuration = configuration;
            _linkGenerator = linkGenerator;
            _imageService = imageService;
            _logger = logger;
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

            var folderName = Guid.NewGuid().ToString();
            string _uploadDirctory = _configuration["UploadDirectory"] ?? "uploads";

            var folderPath = Path.Combine(_uploadDirctory, "images",folderName);
            var fileName =$"{folderName}{Path.GetExtension(file.FileName)}";

            var originalFilePath = await _imageService.SaveOriginalImageAsync(file, folderPath, fileName);
            await _imageService.GenerateThumbnailsAsync(originalFilePath, folderPath, folderName);

            var thumbnailLinks = ImageService.ThumbnailWidths.ToDictionary(
                width => $"w{width}",
                width => GetFullyQualifiedUrl(nameof(GetImage), new { id = folderName, width }));
            
            thumbnailLinks.Add("original",GetFullyQualifiedUrl(nameof(GetImage),new { id = folderName }));

            return Ok(new {id=folderName, links = thumbnailLinks});

        }

        [HttpGet]
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
