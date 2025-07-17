using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ZvezdoGpt.Blazor.Services;

namespace ZvezdoGpt.Blazor.Pages;

public partial class Settings
{
    [Inject] private IJSRuntime JS { get; set; }

    [Inject] private ApiKeyService ApiKeyService { get; set; }

    [Inject] private PreferredModelService PreferredModelService { get; set; }

    [Inject] private AvailableModelsInitializer AvailableModelsInitializer { get; set; }

    private HashSet<string> availableModels = [];
    private string selectedModel;
    private string apiKey;

    protected override Task OnInitializedAsync() => AvailableModelsInitializer.Initialize(
        models => availableModels = models,
        model =>
        {
            selectedModel = model;
            StateHasChanged();
        });

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

    private async Task SavePreferredModel()
    {
        var preferredModel = await PreferredModelService.GetValue();
        if (preferredModel != selectedModel)
        {
            await PreferredModelService.SetValue(selectedModel);
        }
    }
}
