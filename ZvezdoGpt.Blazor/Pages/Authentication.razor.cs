using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using ZvezdoGpt.Blazor.Services;
using ZvezdoGpt.Blazor.Utils;

namespace ZvezdoGpt.Blazor.Pages;

public partial class Authentication
{
    [Parameter] public string Action { get; set; }

    [Inject] private ApiKeyService ApiKeyService { get; set; }

    [Inject] private PreferredModelService PreferredModelService { get; set; }

    [Inject] private IHttpClientFactory HttpClientFactory { get; set; }

    private async Task OnLogInSucceeded(RemoteAuthenticationState state)
    {
        try
        {
            var tasks = new List<Task>();
            var client = HttpClientFactory.CreateClient();

            if (!string.IsNullOrEmpty(await ApiKeyService.GetLocalStorageValue()))
            {
                tasks.Add(client.PostApiKey());
            }

            var localPreferredModel = await PreferredModelService.GetLocalStorageValue();
            if (!string.IsNullOrEmpty(localPreferredModel))
            {
                tasks.Add(client.PostPreferredModel(localPreferredModel));
            }
            else
            {
                tasks.Add(GetServerPreferredModel(client));                
            }

            await Task.WhenAll(tasks);
        }
        catch
        {
        }        
    }

    private async Task GetServerPreferredModel(HttpClient client)
    {
        var serverPreferredModel = await client.GetPreferredModel();
        if (!string.IsNullOrEmpty(serverPreferredModel))
        {
            await PreferredModelService.SetLocalOnly(serverPreferredModel);
        }
    }
}
