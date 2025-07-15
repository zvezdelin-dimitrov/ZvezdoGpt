using Microsoft.AspNetCore.Components.Authorization;
using ZvezdoGpt.Blazor.Utils;

namespace ZvezdoGpt.Blazor.Services;

internal class PreferredModelService(LocalStorageService localStorage, IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationProvider)
    : UserPreferenceService(localStorage, httpClientFactory, authenticationProvider)
{
    protected override string CacheKey => "preferredModel";

    protected override string DefaultValue => "gpt-4.1-nano";

    protected override Task PostToServer(HttpClient client, string value) => client.PostPreferredModel(value);
}
