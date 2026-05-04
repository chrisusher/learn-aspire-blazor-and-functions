#:sdk Aspire.AppHost.Sdk@13.2.4
#:package Aspire.Hosting.Azure.Functions@13.2.4
#:package Aspire.Hosting.Azure.Storage@13.2.4
#:package Aspire.Hosting.Foundry@13.2.4-preview.1.26224.4
#:project ../Backend/Backend.csproj
#:project ../UI/UI.csproj

using Aspire.Hosting.Foundry;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(azurite =>
    {
        azurite.WithDataVolume("data");
    });

var foundry = builder.AddFoundry("foundry");
var project = foundry.AddProject("weather-project");
var model = foundry.AddDeployment("chat", FoundryModel.OpenAI.Gpt54Mini);

var api = builder.AddAzureFunctionsProject("api", "../Backend/Backend.csproj")
    .WaitFor(storage)
    .WithReference(project)
    .WithReference(model)
    .WithArgs("--verbose", "--script-root", @"..\..\..")
    .WithHostStorage(storage)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.UI>("web")
    .WithReference(api)
    .WithReference(project)
    .WithReference(model)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
