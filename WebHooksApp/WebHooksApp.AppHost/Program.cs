var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
                       .WithDataVolume()
                       .WithPgAdmin()
                       .AddDatabase("webhooks");

builder.AddProject<Projects.Orders_Api>("orders-api")
        .WithReference(postgres)
        .WaitFor(postgres);

builder.Build().Run();
