using Bal.Converter.Messages;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.Modules.Settings.Views;
using Bal.Converter.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

namespace Bal.Converter.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    private readonly ILocalSettingsService localSettingsService;

    [ObservableProperty] private bool isInteractionEnabled;
    [ObservableProperty] private bool isBackEnabled;
    [ObservableProperty] private object? selected;
    [ObservableProperty] private int downloadCount;

    public ShellViewModel(INavigationService navigationService,
                          INavigationViewService navigationViewService,
                          IDownloadsRegistryService downloadsRegistry,
                          ILocalSettingsService localSettingsService)
    {
        this.localSettingsService = localSettingsService;
        this.NavigationService = navigationService;
        this.NavigationViewService = navigationViewService;

        this.NavigationService.Navigated += this.OnNavigated;

        this.IsInteractionEnabled = true;
        this.DownloadCount = downloadsRegistry.AllJobs.Count(x => x.State != DownloadState.Cancelled || x.State != DownloadState.Cancelled);

        WeakReferenceMessenger.Default.Register<DisableInteractionChangeMessage>(this, (recipient, message) => this.IsInteractionEnabled = !message.Value);
        WeakReferenceMessenger.Default.Register<DownloadAddedMessage>(this, (recipient, message) => this.DownloadCount++);
        WeakReferenceMessenger.Default.Register<DownloadRemovedMessage>(this, this.OnDownloadRemoved);
        WeakReferenceMessenger.Default.Register<DownloadStateMessage>(this, this.OnDownloadStateChanged);
    }

    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    [RelayCommand]
    private void OpenWindow()
    {
        if (!App.MainWindow.Visible)
        {
            App.MainWindow.Show();
            WeakReferenceMessenger.Default.Send(new WindowStateChangedMessage(true));
        }
    }

    [RelayCommand]
    private async Task CloseApp()
    {
        bool firstTime = await this.localSettingsService.ReadSettingsAsync<bool>(ILocalSettingsService.FirstTimeKey);

        if (!firstTime)
        {
            App.ForceQuit = true;
        }

        Application.Current.Exit();
    }

    private void OnDownloadRemoved(object recipient, DownloadRemovedMessage message)
    {
        if (--this.DownloadCount <= 0)
        {
            this.DownloadCount = 0;
        }
    }

    private void OnDownloadStateChanged(object recipient, DownloadStateMessage message)
    {
        switch (message.Value.State)
        {
            case DownloadState.Cancelled:
            case DownloadState.Done:
                this.DownloadCount--;
                break;
        }
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        this.IsBackEnabled = this.NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            this.Selected = this.NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = this.NavigationViewService.GetSelectedItem(e.SourcePageType);

        if (selectedItem != null)
        {
            this.Selected = selectedItem;
        }
    }
}
