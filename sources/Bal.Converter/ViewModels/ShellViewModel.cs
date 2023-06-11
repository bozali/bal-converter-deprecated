using Bal.Converter.Messages;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.Modules.Settings.Views;
using Bal.Converter.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.UI.Xaml.Navigation;

namespace Bal.Converter.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty] private bool isInteractionEnabled;
    [ObservableProperty] private bool isBackEnabled;
    [ObservableProperty] private object? selected;
    [ObservableProperty] private int downloadCount;

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService, IDownloadsRegistryService downloadsRegistry)
    {
        this.NavigationService = navigationService;
        this.NavigationViewService = navigationViewService;

        this.NavigationService.Navigated += this.OnNavigated;

        this.IsInteractionEnabled = true;
        this.DownloadCount = downloadsRegistry.AllJobs.Count(x => x.State != DownloadState.Cancelled || x.State != DownloadState.Cancelled);

        WeakReferenceMessenger.Default.Register<DisableInteractionChangeMessage>(this, (recipient, message) => this.IsInteractionEnabled = !message.Value);
        WeakReferenceMessenger.Default.Register<DownloadAddedMessage>(this, (recipient, message) => this.DownloadCount++);
        WeakReferenceMessenger.Default.Register<DownloadRemovedMessage>(this, (recipient, message) =>
        {
            if (--this.DownloadCount <= 0)
            {
                this.DownloadCount = 0;
            }
        });

        WeakReferenceMessenger.Default.Register<DownloadStateMessage>(this, (recipient, message) =>
        {
            switch (message.Value.State)
            {
                case DownloadState.Cancelled:
                case DownloadState.Done:
                    this.DownloadCount--;
                    break;
            }
        });
    }

    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

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
