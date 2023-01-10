using Bal.Converter.Modules.Settings.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Bal.Converter.Modules.Settings.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        this.ViewModel = App.GetService<SettingsViewModel>();

        this.InitializeComponent();
    }

    public SettingsViewModel ViewModel { get; }
}