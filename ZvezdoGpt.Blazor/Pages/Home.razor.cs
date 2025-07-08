using Microsoft.AspNetCore.Components;
using OpenAI;
using OpenAI.Chat;
using ZvezdoGpt.Blazor.Services;

namespace ZvezdoGpt.Blazor.Pages;

public partial class Home
{
    [Inject] private OpenAIClient OpenAIClient { get; set; }

    [Inject] private AvailableModelsInitializer AvailableModelsInitializer { get; set; }

    private readonly List<ChatMessage> messages = [];
    private readonly List<ChatMessageContentPart> currentResponses = [];
    private string currentInput;

    private readonly HashSet<string> availableModels = [];
    private string selectedModel;

    protected override async Task OnInitializedAsync()
    {
        await AvailableModelsInitializer.Initialize(availableModels);
        selectedModel = availableModels.FirstOrDefault();
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
