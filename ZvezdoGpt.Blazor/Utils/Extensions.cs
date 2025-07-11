using Markdig;
using OpenAI.Chat;
using System.Net.Http.Json;

namespace ZvezdoGpt.Blazor.Utils;

internal static class Extensions
{
    private static string ToMarkdownHtml(this string markdown) => Markdown.ToHtml(markdown ?? string.Empty);

    public static string ToMarkdowHtml(this ChatMessage message)
        => ((message is UserChatMessage ? "**You:** " : "**ZvezdoGpt:** ")
            + string.Join(string.Empty, message.Content.Select(x => x.Text))).ToMarkdownHtml();

    public static string ToMarkdownHtml(this List<ChatMessageContentPart> responses)
        => ("**ZvezdoGpt:** " + string.Join(string.Empty, responses.Select(x => x.Text))).ToMarkdownHtml();

    public static Task PostApiKey(this HttpClient client) => client.PostAsync("user/apikey", null);

    public static Task PostPreferredModel(this HttpClient client, string model) => client.PostAsJsonAsync("user/preferred-model", model);

    public static Task<string> GetPreferredModel(this HttpClient client) => client.GetStringAsync("user/preferred-model");

}
