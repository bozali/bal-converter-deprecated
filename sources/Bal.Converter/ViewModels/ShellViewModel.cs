using Bal.Converter.Messages;
using Bal.Converter.Modules.Settings.Views;
using Bal.Converter.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Navigation;

namespace Bal.Converter.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty] private bool isInteractionEnabled;

    private bool isBackEnabled;
    private object? selected;

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        this.NavigationService = navigationService;
        this.NavigationViewService = navigationViewService;

        this.NavigationService.Navigated += this.OnNavigated;

        this.IsInteractionEnabled = true;

        WeakReferenceMessenger.Default.Register<DisableInteractionChangeMessage>(this, (recipient, message) => this.IsInteractionEnabled = !message.Value);
    }

    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    public bool IsBackEnabled
    {
        get => isBackEnabled;
        set => this.SetProperty(ref isBackEnabled, value);
    }

    public object? Selected
    {
        get => selected;
        set => this.SetProperty(ref selected, value);
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
