using Bal.Converter.Contracts.Services;
using Bal.Converter.Modules.Settings.Views;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Navigation;

namespace Bal.Converter.ViewModels;

public class ShellViewModel : ObservableRecipient
{
    private bool isBackEnabled;
    private object? selected;

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        this.NavigationService = navigationService;
        this.NavigationViewService = navigationViewService;

        this.NavigationService.Navigated += this.OnNavigated;
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
