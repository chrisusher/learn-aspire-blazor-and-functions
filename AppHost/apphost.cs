#:sdk Aspire.AppHost.Sdk@13.2.4
#:package Aspire.Hosting.Azure.Functions@13.2.4
#:package Aspire.Hosting.Azure.Storage@13.2.4
#:project ../Backend/Backend.csproj
#:project ../UI/UI.csproj

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(azurite =>
    {
        azurite.WithDataVolume("data");
    });

var api = builder.AddAzureFunctionsProject("api", "../Backend/Backend.csproj")
    .WaitFor(storage)
    .WithArgs("--verbose", "--script-root", @"..\..\..")
    .WithHostStorage(storage)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.UI>("web")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
