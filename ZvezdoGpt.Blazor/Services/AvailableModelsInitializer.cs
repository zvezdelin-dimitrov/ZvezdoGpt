using OpenAI;

namespace ZvezdoGpt.Blazor.Services;

internal class AvailableModelsInitializer(OpenAIClient openAIClient, PreferredModelService preferredModelService)
{
    public async Task Initialize(HashSet<string> availableModels)
    {
        availableModels.Add(await preferredModelService.GetValue());

        var modelsFromServer = (await openAIClient.GetOpenAIModelClient().GetModelsAsync()).Value.Select(m => m.Id).ToHashSet();

        availableModels.RemoveWhere(m => !modelsFromServer.Contains(m));

        foreach (var model in modelsFromServer)
        {
            availableModels.Add(model);
        }
    }
}
