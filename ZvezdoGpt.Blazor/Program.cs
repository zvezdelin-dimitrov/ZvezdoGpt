using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OpenAI;
using System.ClientModel;
using System.ClientModel.Primitives;
using ZvezdoGpt.Blazor;
using ZvezdoGpt.Blazor.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);

    options.UserOptions.NameClaim = "preferred_username";
});

builder.Services.AddScoped<OptionalAuthorizationMessageHandler>();
builder.Services.AddScoped<ApiKeyMessageHandler>();

const string httpClientName = "httpClient";

builder.Services.AddHttpClient(httpClientName)
                .AddHttpMessageHandler(sp => sp.GetRequiredService<OptionalAuthorizationMessageHandler>())
                .AddHttpMessageHandler(sp => sp.GetRequiredService<ApiKeyMessageHandler>());

builder.Services.AddSingleton(serviceProvider => 
    new OpenAIClient(
        new ApiKeyCredential(" "), 
        new OpenAIClientOptions
        {
            Transport = new HttpClientPipelineTransport(serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(httpClientName)),
            Endpoint = new Uri(builder.Configuration["ApiUrl"])
        })
    .GetChatClient("gpt-4.1-nano"));

await builder.Build().RunAsync();
