using Microsoft.JSInterop;

namespace Juntos_A_Suerte_Wasm.Services;

public class AuthService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly string _apiBaseUrl;

    public AuthService(IJSRuntime jsRuntime, string apiBaseUrl)
    {
        _jsRuntime = jsRuntime;
        _apiBaseUrl = apiBaseUrl.TrimEnd('/');
    }

    public record AuthUser(string id, string name, string login, string avatar);

    public async Task<AuthUser?> GetMeAsync()
    {
        return await _jsRuntime.InvokeAsync<AuthUser?>("auth.getMe", _apiBaseUrl);
    }

    public string GetGitHubLoginUrl()
    {
        return _apiBaseUrl + "/auth/login/github";
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("auth.logout", _apiBaseUrl);
    }
}

