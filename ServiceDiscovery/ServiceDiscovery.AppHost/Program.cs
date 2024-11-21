var builder = DistributedApplication.CreateBuilder(args);

// First Api - Simple Weather Service
var firstApi = builder.AddProject<Projects.webapi_1>("first");

// Second Api - Weather Sevice Api calling First Api using HttpClient. No other fun logic
var secondApi = builder.AddProject<Projects.webapi_2>("second")
                       .WithReference(firstApi);
// Reverse proxy 
builder.AddProject<Projects.ReverseProxyService>("reverseproxyservice")
       .WithExternalHttpEndpoints()
       .WithReference(secondApi);

builder.Build().Run();
