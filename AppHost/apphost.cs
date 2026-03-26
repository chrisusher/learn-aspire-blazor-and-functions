#:sdk Aspire.AppHost.Sdk@13.2.0
#:project ../UI/UI.csproj

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.UI>("web")
    .WithExternalHttpEndpoints();

builder.Build().Run();
