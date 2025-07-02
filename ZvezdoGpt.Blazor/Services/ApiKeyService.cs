using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ZvezdoGpt.Blazor.Services;

internal class ApiKeyService(IJSRuntime js, IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationProvider) 
    : UserPreferenceService(js, httpClientFactory, authenticationProvider)
{
    protected override string CacheKey => "apiKey";

    protected override Task PostToServerAsync(HttpClient client, string value) => client.PostAsync("user/apikey", null);
}
