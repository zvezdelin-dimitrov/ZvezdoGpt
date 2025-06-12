using ZvezdoGpt.Blazor.Services;

namespace ZvezdoGpt.Blazor.Handlers;

internal class ApiKeyMessageHandler(ApiKeyService apiKeyService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var apiKey = await apiKeyService.GetApiKey();

        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            request.Headers.Add("X-API-KEY", apiKey);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
