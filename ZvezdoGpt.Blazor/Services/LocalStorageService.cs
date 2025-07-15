using Microsoft.JSInterop;

namespace ZvezdoGpt.Blazor.Services;

internal class LocalStorageService(IJSRuntime js)
{
    public ValueTask<string> GetItem(string key) => js.InvokeAsync<string>("localStorage.getItem", key);

    public ValueTask SetItem(string key, string value) => js.InvokeVoidAsync("localStorage.setItem", key, value);
}
