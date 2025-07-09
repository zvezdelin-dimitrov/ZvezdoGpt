using OpenAI;

namespace ZvezdoGpt.Blazor.Services;

internal class AvailableModelsInitializer(OpenAIClient openAIClient, PreferredModelService preferredModelService)
{
    private static HashSet<string> availableModels;

    public async Task Initialize(Action<HashSet<string>> setAvailableModels, Action<string> setSelectedModel)
    {
        if (availableModels is null)
        {
            var preferredModel = await preferredModelService.GetValue();
            setAvailableModels([preferredModel]);
            setSelectedModel(preferredModel);

            availableModels = (await openAIClient.GetOpenAIModelClient().GetModelsAsync()).Value.Select(m => m.Id).ToHashSet();
        }

        setAvailableModels(availableModels);
        setSelectedModel(availableModels.FirstOrDefault());
    }
}
