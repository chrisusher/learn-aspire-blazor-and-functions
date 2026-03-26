#:sdk Aspire.AppHost.Sdk@13.2.0
#:package Aspire.Hosting.Azure.Functions@*
#:package Aspire.Hosting.Azure.Storage@*
#:project ../Backend/Backend.csproj
#:project ../UI/UI.csproj

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddAzureFunctionsProject<Projects.Backend>("api")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.UI>("web")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
