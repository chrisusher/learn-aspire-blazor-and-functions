using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functions.Functions;

public class GetWeatherForecast
{
    string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    private readonly ILogger<GetWeatherForecast> _logger;

    public GetWeatherForecast(ILogger<GetWeatherForecast> logger)
    {
        _logger = logger;
    }

    [Function("GetWeatherForecast")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "weatherforecast")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
        return new JsonResult(forecast);
    }

    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}