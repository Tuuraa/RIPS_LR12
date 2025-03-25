var builder = DistributedApplication.CreateBuilder(args);


var apiService = builder.AddProject<Projects.LR12_ApiService>("apiservice");

builder.AddProject<Projects.LR12_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
