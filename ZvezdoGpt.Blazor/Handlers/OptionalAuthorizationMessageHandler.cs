using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;

namespace ZvezdoGpt.Blazor.Handlers;

internal class OptionalAuthorizationMessageHandler(IAccessTokenProvider provider, IConfiguration configuration) : DelegatingHandler
{
    private readonly AccessTokenRequestOptions options = new() { Scopes = [configuration["ApiRequiredScopes"]] };

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var tokenResult = await provider.RequestAccessToken(options);

        if (tokenResult.TryGetToken(out var token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
