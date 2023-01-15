using Bal.Converter.Activation;
using Bal.Converter.Contracts.Services;
using Bal.Converter.Modules.Downloads.ViewModels;
using Bal.Converter.Modules.Downloads.Views;
using Bal.Converter.Modules.MediaDownloader.ViewModels;
using Bal.Converter.Modules.MediaDownloader.Views;
using Bal.Converter.Modules.Settings.ViewModels;
using Bal.Converter.Modules.Settings.Views;
using Bal.Converter.Services;
using Bal.Converter.ViewModels;
using Bal.Converter.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace Bal.Converter;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        this.Host = Microsoft.Extensions.Hosting.Host
                             .CreateDefaultBuilder()
                             .UseContentRoot(AppContext.BaseDirectory)
                             .ConfigureServices((context, services) =>
                             {
                                 // Default Activation Handler
                                 services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

                                 // Other Activation Handlers

                                 // Services
                                 services.AddTransient<INavigationViewService, NavigationViewService>();

                                 services.AddSingleton<IActivationService, ActivationService>();
                                 services.AddSingleton<INavigationService, NavigationService>();
                                 services.AddSingleton<IPageService, PageService>();

                                 // Views and ViewModels
                                 services.AddTransient<MainViewModel>()
                                         .AddTransient<MediaDownloaderViewModel>()
                                         .AddTransient<DownloadsViewModel>()
                                         .AddTransient<SettingsViewModel>()
                                         .AddTransient<ShellViewModel>()

                                         .AddTransient<MediaDownloaderPage>()
                                         .AddTransient<DownloadsPage>()
                                         .AddTransient<SettingsPage>()
                                         .AddTransient<MainPage>()
                                         .AddTransient<ShellPage>();
                             }).
                             Build();

        this.UnhandledException += this.OnUnhandledException;
    }
    
    public IHost Host { get; }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static T GetService<T>() where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    private void OnUnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
