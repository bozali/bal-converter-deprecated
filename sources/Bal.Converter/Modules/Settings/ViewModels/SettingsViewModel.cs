using ABI.Microsoft.UI.Windowing;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Windows.Storage.Pickers;

namespace Bal.Converter.Modules.Settings.ViewModels;

public partial class SettingsViewModel : ObservableRecipient, INavigationAware
{
    [ObservableProperty] private string? downloadDirectory;
    [ObservableProperty] private bool minimize;
    [ObservableProperty] private string? bandwidth;
    [ObservableProperty] private string? bandwidthMinimized;

    private readonly ILocalSettingsService localSettingsService;

    public SettingsViewModel(ILocalSettingsService localSettingsService)
    {
        this.localSettingsService = localSettingsService;
    }

    public void OnNavigatedTo(object parameter)
    {
        this.DownloadDirectory = this.localSettingsService.ReadSettings<string>(ILocalSettingsService.DownloadDirectoryKey);
        this.Minimize = this.localSettingsService.ReadSettings<bool>(ILocalSettingsService.MinimizeAppKey);
        this.Bandwidth = this.localSettingsService.ReadSettings<string>(ILocalSettingsService.BandwidthKey);
        this.BandwidthMinimized = this.localSettingsService.ReadSettings<string>(ILocalSettingsService.BandwidthMinimizedKey);
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private async Task ChangeDownloadDirectory()
    {
        string? newPath = await PickSingleFolderDialog();

        if (!string.IsNullOrEmpty(newPath))
        {
            this.DownloadDirectory = newPath;
        }
    }

    private static async Task<string?> PickSingleFolderDialog()
    {
        var openPicker = new FolderPicker();
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);
        openPicker.FileTypeFilter.Add("*");
        var folder = await openPicker.PickSingleFolderAsync();
        return folder?.Path;
    }

    partial void OnDownloadDirectoryChanged(string? value)
    {
        this.localSettingsService.SaveSettingsAsync(ILocalSettingsService.DownloadDirectoryKey, value).ConfigureAwait(false);
    }

    partial void OnMinimizeChanged(bool value)
    {
        this.localSettingsService.SaveSettingsAsync(ILocalSettingsService.MinimizeAppKey, value).ConfigureAwait(false);
    }

    partial void OnBandwidthChanged(string? value)
    {
        this.localSettingsService.SaveSettingsAsync(ILocalSettingsService.BandwidthKey, !string.IsNullOrEmpty(value) ? int.Parse(value) : 0).ConfigureAwait(false);
    }

    partial void OnBandwidthMinimizedChanged(string? value)
    {
        this.localSettingsService.SaveSettingsAsync(ILocalSettingsService.BandwidthMinimizedKey, !string.IsNullOrEmpty(value) ? int.Parse(value) : 0).ConfigureAwait(false);
    }

}