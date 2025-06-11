using Microsoft.AspNetCore.Components;
using OpenAI;
using OpenAI.Chat;

namespace ZvezdoGpt.Blazor.Pages;

public partial class Home
{
    [Inject] private OpenAIClient OpenAIClient { get; set; }    
    
    private readonly List<ChatMessage> messages = [];
    private readonly List<ChatMessageContentPart> currentResponses = [];
    private string currentInput;

    private readonly HashSet<string> availableModels = ["gpt-4.1-nano"];
    private string selectedModel = "gpt-4.1-nano";

    protected override async Task OnInitializedAsync()
    {
        var models = (await OpenAIClient.GetOpenAIModelClient().GetModelsAsync()).Value.Select(m => m.Id).ToHashSet();

        availableModels.RemoveWhere(m => !models.Contains(m));

        foreach (var model in models)
        {
            availableModels.Add(model);
        }
    }

    private async Task SendMessage()
    {
        messages.Add(new UserChatMessage(currentInput));
        currentInput = null;

        await foreach (var completionUpdate in OpenAIClient.GetChatClient(selectedModel).CompleteChatStreamingAsync(messages))
        {
            currentResponses.AddRange(completionUpdate.ContentUpdate.Where(x => x.Kind is ChatMessageContentPartKind.Text));
            
            await InvokeAsync(StateHasChanged);
            await Task.Delay(1);
        }

        messages.Add(new AssistantChatMessage(currentResponses));
        currentResponses.Clear();
    }

    private Task OnInputSubmit()
    {
        if (!string.IsNullOrWhiteSpace(currentInput))
        {
            return SendMessage();
        }

        return Task.CompletedTask;
    }
}
