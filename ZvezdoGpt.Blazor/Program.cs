using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OpenAI;
using ZvezdoGpt.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);

    options.UserOptions.NameClaim = "preferred_username";
});

builder.Services.AddSingleton(new OpenAIClient("").GetChatClient("gpt-4.1-nano"));

await builder.Build().RunAsync();
