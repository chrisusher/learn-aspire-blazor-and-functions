using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var config = new ConfigurationBuilder()
#if DEBUG
.AddJsonFile("local.settings.json", false)
#endif
.AddEnvironmentVariables().Build();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };

        worker.Serializer = new JsonObjectSerializer(options);
    })
    .ConfigureOpenApi()
    .ConfigureServices(services =>
    {
        // Add Application Insights telemetry for Azure Functions (isolated worker model).
        services.ConfigureFunctionsApplicationInsights();
        services.AddApplicationInsightsTelemetryWorkerService();

        // Configure logging to suppress Azure Storage noise
        services.Configure<LoggerFilterOptions>(options =>
        {
            // Add additional filters for Azure Storage SDK logs
            options.AddFilter("Azure.Storage.Blobs", LogLevel.Error);
            options.AddFilter("Azure.Storage.Common", LogLevel.Error);
            options.AddFilter("Azure.Core", LogLevel.Error);
            options.AddFilter("Azure", LogLevel.Error);
            options.AddFilter("Microsoft.Azure.Storage", LogLevel.Error);
            options.AddFilter("Microsoft.Azure.WebJobs.Host.Blobs", LogLevel.Error);
            options.AddFilter("Microsoft.Azure.WebJobs.Extensions.Storage", LogLevel.Error);
        });

        // Add HttpClient for Clerk token validation
        services.AddHttpClient();
    })
    .Build();

host.Run();