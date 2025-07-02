using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace ZvezdoGpt.Blazor.Services;

internal class PreferredModelService(IJSRuntime js, IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationProvider)
    : UserPreferenceService(js, httpClientFactory, authenticationProvider)
{
    protected override string CacheKey => "preferredModel";

    protected override string DefaultValue => "gpt-4.1-nano";

    protected override Task PostToServerAsync(HttpClient client, string value) => client.PostAsJsonAsync("user/preferred-model", value);
}
