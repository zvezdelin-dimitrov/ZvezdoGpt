using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ZvezdoGpt.Blazor.Services;

internal class ApiKeyService(IJSRuntime js, IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationProvider)
{
    private static string apiKey;

    public async ValueTask<string> GetApiKey()
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            apiKey = await js.InvokeAsync<string>("localStorage.getItem", nameof(apiKey));
        }
        
        return apiKey;
    }

    public Task SetApiKey(string key)
    {
        apiKey = key;

        return Task.WhenAll(SaveApiKeyInLocalStorage(), SaveApiKeyOnServer());
    }

    private Task SaveApiKeyInLocalStorage()
        => js.InvokeVoidAsync("localStorage.setItem", nameof(apiKey), apiKey).AsTask();

    private async Task SaveApiKeyOnServer()
    {
        try
        {
            var authState = await authenticationProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated)
            {
                await httpClientFactory.CreateClient(Constants.HttpClientName).PostAsync("user/apikey", null);
            }
        }
        catch
        {
        }
    }
}
