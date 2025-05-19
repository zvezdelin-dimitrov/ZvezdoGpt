using Microsoft.AspNetCore.Components;
using OpenAI.Chat;

namespace ZvezdoGpt.Blazor.Pages;

public partial class Home
{
    [Inject] private ChatClient ChatClient { get; set; }    
    
    private readonly List<ChatMessage> messages = [];
    private readonly List<ChatMessageContentPart> currentResponses = [];
    private string currentInput;

    private async Task SendMessage()
    {
        messages.Add(new UserChatMessage(currentInput));
        currentInput = null;

        await foreach (var completionUpdate in ChatClient.CompleteChatStreamingAsync(messages))
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
