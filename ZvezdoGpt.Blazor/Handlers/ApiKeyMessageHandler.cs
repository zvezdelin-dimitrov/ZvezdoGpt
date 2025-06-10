namespace ZvezdoGpt.Blazor.Handlers;

public class ApiKeyMessageHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("X-API-KEY", "");
        return base.SendAsync(request, cancellationToken);
    }
}
