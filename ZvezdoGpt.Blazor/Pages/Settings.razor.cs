using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ZvezdoGpt.Blazor.Services;

namespace ZvezdoGpt.Blazor.Pages;

public partial class Settings
{
    [Inject] private IJSRuntime JS { get; set; }

    [Inject] private ApiKeyService ApiKeyService { get; set; }

    private string apiKey;

    private async Task SaveApiKey()
    {
        var existingKey = await ApiKeyService.GetValue();

        if (existingKey == apiKey
            || (!string.IsNullOrWhiteSpace(existingKey)
                && !await JS.InvokeAsync<bool>("confirm", "Do you want to overwrite your saved API key?")))
        {
            return;
        }

        await ApiKeyService.SetValue(apiKey);
        apiKey = null;
    }
}
