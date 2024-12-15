
using System.Collections.Concurrent;
using System.Threading.Channels;
using ThumbnailGenerator.Models;

namespace ThumbnailGenerator.Services;

public class ThumbnailGenerationService : BackgroundService
{
    private readonly ILogger<ThumbnailGenerationService> _logger;
    private readonly ImageService _imageService;
    private readonly Channel<ThumbnailGeneratorJob> _channel;
    private readonly ConcurrentDictionary<string, ThumbnailGenerationStatus> _statusDictionary;

    public ThumbnailGenerationService(ILogger<ThumbnailGenerationService> logger,
                                       ImageService imageService,
                                       Channel<ThumbnailGeneratorJob> channel,
                                       ConcurrentDictionary<string, ThumbnailGenerationStatus> statusDictionary)
    {
        _logger = logger;
        _imageService = imageService;
        _channel = channel;
        _statusDictionary = statusDictionary;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //Get all the message from the Channel and process
        await foreach(var job in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await ProcessJobAsync(job);
            }
            catch(OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in job Generating Thumbnails {exception}", ex.ToString());
                throw;
            }
        }
    }

    private async Task ProcessJobAsync(ThumbnailGeneratorJob job)
    {
        _statusDictionary[job.Id] = ThumbnailGenerationStatus.Processing;

        try
        {
            await _imageService.GenerateThumbnailsAsync(job.originalFilePath, job.FolderPath, job.Id);
            _statusDictionary[job.Id] = ThumbnailGenerationStatus.Completed;
        }
        catch (Exception ex)
        {
            _statusDictionary[job.Id] = ThumbnailGenerationStatus.Failed;

            Console.WriteLine(ex.ToString());
            _logger.LogError("Error processing Image generation job {exception}",ex.ToString());
            throw;
        }
    }
}
