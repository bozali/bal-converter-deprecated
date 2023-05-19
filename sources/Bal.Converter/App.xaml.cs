using Bal.Converter.Activation;
using Bal.Converter.Common.Conversion;
using Bal.Converter.Common.Conversion.Extensions;
using Bal.Converter.Common.Services;
using Bal.Converter.Common.Web;
using Bal.Converter.Contracts.Services;
using Bal.Converter.Extensions;
using Bal.Converter.FFmpeg;
using Bal.Converter.Modules.Conversion.Filters.Volume;
using Bal.Converter.Modules.Conversion.Filters.Watermark;
using Bal.Converter.Modules.Conversion.Image.Settings.Ico;
using Bal.Converter.Modules.Conversion.Image.ViewModels;
using Bal.Converter.Modules.Conversion.Image.Views;
using Bal.Converter.Modules.Conversion.Video.View;
using Bal.Converter.Modules.Conversion.Video.ViewModels;
using Bal.Converter.Modules.Conversion.View;
using Bal.Converter.Modules.Conversion.ViewModels;
using Bal.Converter.Modules.Downloads.ViewModels;
using Bal.Converter.Modules.Downloads.Views;
using Bal.Converter.Modules.MediaDownloader.ViewModels;
using Bal.Converter.Modules.MediaDownloader.Views;
using Bal.Converter.Modules.Settings;
using Bal.Converter.Modules.Settings.ViewModels;
using Bal.Converter.Modules.Settings.Views;
using Bal.Converter.Profiles;
using Bal.Converter.Services;
using Bal.Converter.ViewModels;
using Bal.Converter.Views;
using Bal.Converter.Workers;
using Bal.Converter.YouTubeDl;

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
                             .ConfigureServices(this.ConfigureServices)
                             .Build();

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

        await SetupSettings();
        await App.GetService<IActivationService>().ActivateAsync(args);

        // ReSharper disable once ArrangeStaticMemberQualifier
#pragma warning disable CS4014
        App.GetService<FetchBackgroundWorker>().Process().ConfigureAwait(false);
        App.GetService<DownloadBackgroundWorker>().Process().ConfigureAwait(false);
        App.GetService<PlaylistFetchBackgroundWorker>().Process().ConfigureAwait(false);
#pragma warning restore CS4014
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection collection)
    {
        // Default Activation Handler
        collection.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

        // Other Activation Handlers

        // Services
        collection.AddTransient<INavigationViewService, NavigationViewService>();

        collection.AddSingleton<IActivationService, ActivationService>();
        collection.AddSingleton<INavigationService, NavigationService>();
        collection.AddSingleton<ILocalSettingsService, LocalSettingsService>();
        collection.AddSingleton<IPageService, PageService>();
        collection.AddSingleton<IYouTubeDl, YouTubeDl.YouTubeDl>(provider => new YouTubeDl.YouTubeDl(@"Tools\yt-dlp.exe", @"Tools\ffmpeg.exe", ILocalSettingsService.TempPath));
        collection.AddSingleton<IFFmpeg, FFmpeg.FFmpeg>(provider => new FFmpeg.FFmpeg(@"Tools\ffmpeg.exe"));

        collection.AddSingleton<IFileSystemService, FileSystemService>();
        collection.AddSingleton<IFileDownloaderService, FileDownloaderService>();
        collection.AddSingleton<IDownloadsRegistryService, DownloadsRegistryService>();
        collection.AddSingleton<IMediaTagService, MediaTagService>();
        collection.AddSingleton<IConversionProvider, ConversionProvider>();
        collection.AddSingleton<IDialogPickerService, DialogPickerService>();

        collection.ConfigureLiteDatabase();
        collection.ConfigureConversions();

        collection.AddAutoMapper(x => x.AddProfile<BalMapperProfile>());

        // Views and ViewModels
        collection
            .AddTransient<MainViewModel>()
            .AddTransient<ImageConversionEditorViewModel>()
            .AddTransient<VideoConversionEditorViewModel>()
            .AddTransient<ConversionSelectionViewModel>()
            .AddTransient<MediaDownloaderViewModel>()
            .AddTransient<MediaTagEditorViewModel>()
            .AddTransient<DownloadsViewModel>()
            .AddTransient<SettingsViewModel>()
            .AddTransient<ShellViewModel>()
            .AddTransient<PlaylistOverviewViewModel>()
            .AddTransient<VolumeFilterPage>()
            .AddTransient<WatermarkEffectPage>()
            .AddTransient<IcoMultiResolutionPage>()

            .AddTransient<ImageConversionEditorPage>()
            .AddTransient<VideoConversionEditorPage>()
            .AddTransient<ConversionSelectionPage>()
            .AddTransient<MediaDownloaderPage>()
            .AddTransient<MediaTagEditorPage>()
            .AddTransient<PlaylistOverviewPage>()
            .AddTransient<DownloadsPage>()
            .AddTransient<SettingsPage>()
            .AddTransient<MainPage>()
            .AddTransient<ShellPage>()
            .AddTransient<VolumeFilterViewModel>()
            .AddTransient<WatermarkEffectViewModel>()
            .AddTransient<IcoMultiResolutionViewModel>();

        // Background services
        collection.AddSingleton<FetchBackgroundWorker>();
        collection.AddSingleton<DownloadBackgroundWorker>();
        collection.AddSingleton<PlaylistFetchBackgroundWorker>();

        // Configuration
        collection.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
    }

    private static async Task SetupSettings()
    {
        var localSettingsService = App.GetService<ILocalSettingsService>();

        string? downloadDirectory = await localSettingsService.ReadSettingsAsync<string>(ILocalSettingsService.DownloadDirectoryKey);

        if (string.IsNullOrEmpty(downloadDirectory))
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            await localSettingsService.SaveSettingsAsync(ILocalSettingsService.DownloadDirectoryKey, path);
        }

        string? firstTime = await localSettingsService.ReadSettingsAsync<string>(ILocalSettingsService.FirstTimeKey);

        if (string.IsNullOrEmpty(firstTime) || !bool.TryParse(firstTime, out bool firstTimeValue))
        {
            await localSettingsService.SaveSettingsAsync(ILocalSettingsService.FirstTimeKey, true);
        }

        string? minimized = await localSettingsService.ReadSettingsAsync<string>(ILocalSettingsService.MinimizeAppKey);

        if (string.IsNullOrEmpty(minimized) || !bool.TryParse(minimized, out bool minimizedValue))
        {
            await localSettingsService.SaveSettingsAsync(ILocalSettingsService.MinimizeAppKey, true);
        }

        string? bandwidth = await localSettingsService.ReadSettingsAsync<string>(ILocalSettingsService.BandwidthKey);

        if (string.IsNullOrEmpty(bandwidth) || !int.TryParse(bandwidth, out int bandwidthValue))
        {
            await localSettingsService.SaveSettingsAsync(ILocalSettingsService.BandwidthKey, 0);
        }

        string? bandwidthMinimized = await localSettingsService.ReadSettingsAsync<string>(ILocalSettingsService.BandwidthMinimizedKey);

        if (string.IsNullOrEmpty(bandwidthMinimized) || !int.TryParse(bandwidthMinimized, out int bandwidthMinimizedValue))
        {
            await localSettingsService.SaveSettingsAsync(ILocalSettingsService.BandwidthMinimizedKey, 0);
        }
    }
}
