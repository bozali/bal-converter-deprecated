using ABI.Microsoft.UI.Windowing;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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


    [RelayCommand]
    private async Task Save()
    {
        await this.localSettingsService.SaveSettingsAsync(ILocalSettingsService.DownloadDirectoryKey, this.DownloadDirectory);
        await this.localSettingsService.SaveSettingsAsync(ILocalSettingsService.MinimizeAppKey, this.Minimize);
        await this.localSettingsService.SaveSettingsAsync(ILocalSettingsService.BandwidthKey, !string.IsNullOrEmpty(this.Bandwidth) ? int.Parse(this.Bandwidth) : 0);
        await this.localSettingsService.SaveSettingsAsync(ILocalSettingsService.BandwidthMinimizedKey, !string.IsNullOrEmpty(this.BandwidthMinimized) ? int.Parse(this.BandwidthMinimized) : 0);
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
}