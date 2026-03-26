using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using UI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["API_HTTP"] ?? "http://localhost:7287";

builder.Services
	.AddHttpClient("weather-api", client => client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("weather-api"));

builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();
