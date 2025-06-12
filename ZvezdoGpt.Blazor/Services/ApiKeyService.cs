using Microsoft.JSInterop;

namespace ZvezdoGpt.Blazor.Services;

internal class ApiKeyService(IJSRuntime js)
{
    private string apiKey;

    public async ValueTask<string> GetApiKey()
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            apiKey = await js.InvokeAsync<string>("localStorage.getItem", nameof(apiKey));
        }
        
        return apiKey;
    }

    public ValueTask SetApiKey(string key)
    {
        apiKey = key;
        return js.InvokeVoidAsync("localStorage.setItem", nameof(apiKey), key);
    }
}
