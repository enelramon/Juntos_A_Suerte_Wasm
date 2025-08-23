using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Juntos_A_Suerte_Wasm;
using Blazored.LocalStorage;
using Juntos_A_Suerte_Wasm.Services;
using Radzen;
using System.Net.Http.Json;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<NotificationService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<TeamService>();

// Load API base URL from appsettings.json
var bootstrapHttp = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var clientSettings = await bootstrapHttp.GetFromJsonAsync<Dictionary<string, string>>("appsettings.json");
var apiBaseUrl = clientSettings != null && clientSettings.TryGetValue("ApiBaseUrl", out var value) ? value : "http://localhost:5080";

// Register AuthService backed by JS fetch (with credentials)
builder.Services.AddScoped<AuthService>(sp =>
{
    var js = sp.GetRequiredService<IJSRuntime>();
    return new AuthService(js, apiBaseUrl);
});

await builder.Build().RunAsync();
