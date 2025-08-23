using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http;
// using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// CORS: Allow WASM origin from config
var allowedOrigin = builder.Configuration["Frontend:Origin"] ?? "http://localhost:5173";
builder.Services.AddCors(options =>
{
    options.AddPolicy("wasm", policy =>
    {
        policy.WithOrigins(allowedOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "GitHub";
})
.AddCookie(options =>
{
    options.Cookie.Name = "auth";
    options.SlidingExpiration = true;
    options.Cookie.SameSite = SameSiteMode.None;
    // For local dev over HTTP reverse proxy, you may relax this; keep Always in production
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
})
.AddOAuth("GitHub", options =>
{
    options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"] ?? "";
    options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"] ?? "";
    options.CallbackPath = "/auth/github/callback";
    options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
    options.TokenEndpoint = "https://github.com/login/oauth/access_token";
    options.UserInformationEndpoint = "https://api.github.com/user";
    options.SaveTokens = true;
    options.Scope.Add("read:user");
    options.Scope.Add("user:email");

    options.Events = new OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            request.Headers.UserAgent.ParseAdd("JuntosASuerte/1.0");

            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var root = payload.RootElement;

            var id = root.GetProperty("id").GetRawText();
            var login = root.GetProperty("login").GetString() ?? string.Empty;
            var name = root.TryGetProperty("name", out var nameProp) ? nameProp.GetString() ?? login : login;
            var avatarUrl = root.TryGetProperty("avatar_url", out var avatarProp) ? avatarProp.GetString() ?? string.Empty : string.Empty;

            context.Identity!.AddClaim(new Claim(ClaimTypes.NameIdentifier, id));
            context.Identity!.AddClaim(new Claim(ClaimTypes.Name, name));
            context.Identity!.AddClaim(new Claim("urn:github:login", login));
            context.Identity!.AddClaim(new Claim("urn:github:avatar", avatarUrl));
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Trust proxy headers if behind reverse proxy
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors("wasm");
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapGet("/auth/login/github", async (HttpContext http) =>
{
    var front = app.Configuration["Frontend:Origin"] ?? "http://localhost:5173";
    var redirect = $"{front.TrimEnd('/')}/auth/callback";
    return Results.Challenge(new AuthenticationProperties
    {
        RedirectUri = redirect
    }, new[] { "GitHub" });
});

app.MapGet("/auth/logout", async (HttpContext http) =>
{
    await http.SignOutAsync();
    return Results.Ok();
});

app.MapGet("/auth/me", (HttpContext http) =>
{
    if (http.User?.Identity?.IsAuthenticated != true)
    {
        return Results.Unauthorized();
    }

    var name = http.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
    var id = http.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    var login = http.User.FindFirst("urn:github:login")?.Value ?? string.Empty;
    var avatar = http.User.FindFirst("urn:github:avatar")?.Value ?? string.Empty;

    return Results.Json(new { id, name, login, avatar });
});

app.Run();

