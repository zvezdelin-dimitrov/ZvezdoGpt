using Microsoft.AspNetCore.Components.Authorization;
using ZvezdoGpt.Blazor.Utils;

namespace ZvezdoGpt.Blazor.Services;

internal abstract class UserPreferenceService(LocalStorageService localStorage, IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationProvider)
{
    private static readonly Dictionary<string, string> cache = [];

    protected abstract string CacheKey { get; }

    protected virtual string DefaultValue { get; }

    public async ValueTask<string> GetValue()
    {
        if (!cache.TryGetValue(CacheKey, out var value))
        {
            var storedValue = await GetLocalStorageValue();

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

    public ValueTask<string> GetLocalStorageValue() => localStorage.GetItem(CacheKey);

    public Task SetValue(string value) => Task.WhenAll(SetLocalOnly(value).AsTask(), SaveOnServer());

    public ValueTask SetLocalOnly(string value)
    {
        cache[CacheKey] = value;
        return SaveInLocalStorage();
    }

    private ValueTask SaveInLocalStorage() => localStorage.SetItem(CacheKey, cache[CacheKey]);

    private async Task SaveOnServer()
    {
        try
        {
            var authState = await authenticationProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated)
            {
                await PostToServer(httpClientFactory.CreateClient(), cache[CacheKey]);
            }
        }
        catch
        {
        }
    }

    protected abstract Task PostToServer(HttpClient client, string value);
}
