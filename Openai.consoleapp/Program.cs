using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Embeddings;
using ConnectingApps.Refit.OpenAI.Embeddings.Request;
using ConnectingApps.Refit.OpenAI.ImageCreations;
using ConnectingApps.Refit.OpenAI.ImageCreations.Request;
using Microsoft.Extensions.Configuration;
using Refit;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>();

IConfiguration configuraiton = builder.Build();

string api_key = Convert.ToString(configuraiton["OpenAI_Key"]);
System.Console.WriteLine("Perform Embedding using OpenAI Embeddings");

var completionApi = RestService.For<IEmbedding>(new HttpClient
{
    BaseAddress = new Uri("https://api.openai.com")
}, OpenAiRefitSettings.RefitSettings);


var response = await completionApi.GetEmbeddingAsync(new EmbeddingRequest
{
    Input = "The quick brown fox jumped over the lazy dog",
    Model = "text-embedding-ada-002"
}, $"Bearer {api_key}");

Console.WriteLine($"Respose Status : {response.StatusCode}");
Console.WriteLine($"Vector Length :{response.Content!.Data.First().Embedding.Length}");
Console.WriteLine($"Embedding : {string.Join(",", response.Content!.Data.First().Embedding.Take(10))}...");
//Console.ReadKey();

System.Console.WriteLine("Performing Image Creation");

// Image Creation
var completionApi2 = RestService.For<IImageCreation>(new HttpClient
{
    BaseAddress = new Uri("https://api.openai.com")
}, OpenAiRefitSettings.RefitSettings);

var response2 = await completionApi2.CreateImageAsync(new ImageCreationRequest
{
    N=2,
    Prompt = "A girl with a umberlla in a rainy day",
    Size = "1024x1024"    
}, $"Bearer {api_key}");

System.Console.WriteLine($"Returned response Status code:{response2.StatusCode}");
Console.WriteLine($"Images Created count: {response2.Content!.Data.Length}");
Console.WriteLine($"Image Url : {response2.Content!.Data.First().Url}");
Console.WriteLine($"Image Url : {response2.Content!.Data.Last().Url}");
Console.ReadLine();