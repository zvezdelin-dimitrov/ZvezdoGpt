# ZvezdoGpt

ZvezdoGpt is a Blazor WebAssembly application that provides a modern chat interface powered by OpenAI models. Users can interact with AI, manage their API keys, and select preferred models directly from the web UI.

## Features
- Real-time chat interface with markdown rendering
- Model selection from available OpenAI models
- Secure API key management
- User authentication support
- Responsive design using Bootstrap 5

## Technologies Used
- [Blazor WebAssembly](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- .NET 9
- [OpenAI API](https://platform.openai.com/docs/api-reference)
- [Markdig](https://github.com/lunet-io/markdig) for Markdown parsing
- Bootstrap 5 for UI styling

## Project Structure
- `ZvezdoGpt.Blazor/Pages/` - Main Blazor pages (Home, Settings, etc.)
- `ZvezdoGpt.Blazor/Services/` - Service classes for API key and model management
- `ZvezdoGpt.Blazor/Utils/` - Utility and extension methods
- `ZvezdoGpt.Blazor/wwwroot/` - Static assets and index.html

## Configuration
- Set your OpenAI API key in the Settings page after launching the app.
- Choose your preferred model from the available options.
