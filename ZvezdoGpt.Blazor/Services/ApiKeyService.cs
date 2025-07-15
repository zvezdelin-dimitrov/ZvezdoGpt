using Microsoft.AspNetCore.Components.Authorization;
using ZvezdoGpt.Blazor.Utils;

namespace ZvezdoGpt.Blazor.Services;

internal class ApiKeyService(LocalStorageService localStorage, IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationProvider) 
    : UserPreferenceService(localStorage, httpClientFactory, authenticationProvider)
{
    protected override string CacheKey => "apiKey";

    protected override Task PostToServer(HttpClient client, string value) => client.PostApiKey();
}
