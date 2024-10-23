using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Juntos_A_Suerte_Wasm;
using Blazored.LocalStorage;
using Juntos_A_Suerte_Wasm.Services;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<NotificationService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<CartaService>();

await builder.Build().RunAsync();
