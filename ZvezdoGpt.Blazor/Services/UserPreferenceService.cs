using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using ZvezdoGpt.Blazor.Utils;

namespace ZvezdoGpt.Blazor.Services;

internal abstract class UserPreferenceService(IJSRuntime js, IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationProvider)
{
    private static readonly Dictionary<string, string> cache = [];

    protected abstract string CacheKey { get; }

    protected virtual string DefaultValue { get; }

    public async ValueTask<string> GetValue()
    {
        if (!cache.TryGetValue(CacheKey, out var value))
        {
            var storedValue = await js.InvokeAsync<string>("localStorage.getItem", CacheKey);

            if (!string.IsNullOrEmpty(storedValue))
            {
                value = cache[CacheKey] = storedValue;
            }
            else
            {
                value = cache[CacheKey] = DefaultValue;
            }
        }

        return value;
    }

    public Task SetValue(string value)
    {
        cache[CacheKey] = value;

        return Task.WhenAll(SaveInLocalStorage(), SaveOnServer());
    }

    private Task SaveInLocalStorage()
        => js.InvokeVoidAsync("localStorage.setItem", CacheKey, cache[CacheKey]).AsTask();

    private async Task SaveOnServer()
    {
        try
        {
            var authState = await authenticationProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated)
            {
                await PostToServer(httpClientFactory.CreateClient(Constants.HttpClientName), cache[CacheKey]);
            }
        }
        catch
        {
        }
    }

    protected abstract Task PostToServer(HttpClient client, string value);
}
