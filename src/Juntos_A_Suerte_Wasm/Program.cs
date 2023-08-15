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
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<PersonService>(); // Register as a singleton
builder.Services.AddScoped<TeamService>(); // Register as a singleton

await builder.Build().RunAsync();
