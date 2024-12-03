using Scalar.AspNetCore;
using Microsoft.AspNetCore.Http;
using cancellation_token_api;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/long-running-request", async (CancellationToken cancellationToken) =>
{
    var randomId = Ulid.NewUlid();
    var results = new List<string>();
    for (int i = 0; i < 100; i++)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Results.StatusCode(499);
        }

        await Task.Delay(1000);
        var result = $"{randomId} - Result {i}";
        Console.WriteLine(result);
        results.Add(result);
       
    }
    return Results.Ok(results);
})
.WithName("GetAllData")
.WithOpenApi();

app.MapPost("/upload-large-file", async([FromForm] FileUploadRequest request, 
                                    CancellationToken cancellationToken) => 
{
    try
    {
        var s3Client = new AmazonS3Client();
        await s3Client.PutObjectAsync(new PutObjectRequest()
        {
            BucketName = "XXXXXXXXXXX",
            Key = $"{Guid.NewGuid()}-{request.FileName}",
            InputStream = request.File.OpenReadStream()
        }, cancellationToken);
        // await PerformAdditionalTasks(cancellationToken);
        await PerformAdditionalTasks(CancellationToken.None);
        return Results.NoContent();

    }
    catch (Exception ex)
    {
        return Results.StatusCode(499);
    }
})
.WithName("UploadLargeFile")
.DisableAntiforgery()
.WithOpenApi();

async Task PerformAdditionalTasks(CancellationToken cancellationToken)
{
    await Task.Delay(1000, cancellationToken);
    var snsClient = new Amazon.SimpleNotificationService.AmazonSimpleNotificationServiceClient();
    await snsClient.PublishAsync(new Amazon.SimpleNotificationService.Model.PublishRequest()
    {
        Message = "Hello World",
        TopicArn = "XXXXXXXXXXXX"
    }, cancellationToken);
}

app.Run();

