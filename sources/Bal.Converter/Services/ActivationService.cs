using Bal.Converter.Activation;
using Bal.Converter.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bal.Converter.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> defaultHandler;
    private readonly IEnumerable<IActivationHandler> activationHandlers;
    private UIElement? shell;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers)
    {
        this.defaultHandler = defaultHandler;
        this.activationHandlers = activationHandlers;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await InitializeAsync();

        // Set the MainWindow Content.
        if (App.MainWindow.Content == null)
        {
            this.shell = App.GetService<ShellPage>();
            App.MainWindow.Content = this.shell ?? new Frame();
        }

        // Handle activation via ActivationHandlers.
        await this.HandleActivationAsync(activationArgs);

        // Activate the MainWindow.
        App.MainWindow.Activate();

        // Execute tasks after activation.
        await this.StartupAsync();
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = this.activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (defaultHandler.CanHandle(activationArgs))
        {
            await defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        await Task.CompletedTask;
    }
}
