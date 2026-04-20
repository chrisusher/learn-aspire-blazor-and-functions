#:sdk Aspire.AppHost.Sdk@13.2.0
#:package Aspire.Hosting.Azure.CosmosDB@13.2.2
#:package Aspire.Hosting.Azure.Functions@*
#:package Aspire.Hosting.Azure.KeyVault@13.2.2
#:package Aspire.Hosting.Azure.Storage@*
#:project ../Backend/Backend.csproj
#:project ../UI/UI.csproj

var builder = DistributedApplication.CreateBuilder(args);

var existingCosmosName = builder.AddParameter("existingCosmosName");
var existingCosmosResourceGroup = builder.AddParameter("existingCosmosResourceGroup");

var cosmos = builder.AddAzureCosmosDB("cosmos-db")
    .AsExisting(existingCosmosName, existingCosmosResourceGroup);
    
    // Bug - Doesn't work with existing Cosmos DB accounts not in the same resource group as the app.
    // .WithAccessKeyAuthentication();

var db = cosmos.AddCosmosDatabase("db", "my-database");
var container = db.AddContainer("container", "/id", "my-container");

var api = builder.AddAzureFunctionsProject<Projects.Backend>("api")
    .WithReference(container)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.UI>("web")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
