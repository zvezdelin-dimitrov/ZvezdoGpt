using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using ZvezdoGpt.Blazor.Utils;

namespace ZvezdoGpt.Blazor.Services;

internal class ApiKeyService(IJSRuntime js, IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationProvider) 
    : UserPreferenceService(js, httpClientFactory, authenticationProvider)
{
    protected override string CacheKey => "apiKey";

    protected override Task PostToServer(HttpClient client, string value) => client.PostApiKey();
}
