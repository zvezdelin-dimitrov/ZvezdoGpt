using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OpenAI;
using System.ClientModel;
using System.ClientModel.Primitives;
using ZvezdoGpt.Blazor;
using ZvezdoGpt.Blazor.Handlers;
using ZvezdoGpt.Blazor.Services;
using ZvezdoGpt.Blazor.Utils;

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
builder.Services.AddScoped<ApiKeyService>();
builder.Services.AddScoped<PreferredModelService>();
builder.Services.AddScoped<AvailableModelsInitializer>();

var apiUrl = new Uri(builder.Configuration["ApiUrl"]);

builder.Services.AddHttpClient(Constants.HttpClientName, client => client.BaseAddress = apiUrl)
                .AddHttpMessageHandler(sp => sp.GetRequiredService<OptionalAuthorizationMessageHandler>())
                .AddHttpMessageHandler(sp => sp.GetRequiredService<ApiKeyMessageHandler>());

builder.Services.AddSingleton(serviceProvider =>
    new OpenAIClient(
        new ApiKeyCredential(" "),
        new OpenAIClientOptions
        {
            Transport = new HttpClientPipelineTransport(serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(Constants.HttpClientName)),
            Endpoint = apiUrl
        }));

await builder.Build().RunAsync();
