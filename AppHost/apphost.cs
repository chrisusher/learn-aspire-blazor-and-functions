var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .PublishAsContainer();

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator();

var functions = builder.AddAzureFunctionsProject<Projects.Functions>("functions")
    .WithHttpHealthCheck("/health")
    .WithHostStorage(storage)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(functions)
    .WaitFor(functions);

builder.Build().Run();
