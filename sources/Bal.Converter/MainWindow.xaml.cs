using System.Diagnostics;
using Bal.Converter.Extensions;
using Bal.Converter.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Bal.Converter;

public sealed partial class MainWindow : WindowEx
{
    private readonly ILocalSettingsService localSettingsService;

    public MainWindow(ILocalSettingsService localSettingsService)
    {
        this.localSettingsService = localSettingsService;
        this.InitializeComponent();

        this.AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        this.Content = null;
        this.Title = "AppDisplayName".GetLocalized();

        // this.Closed += this.OnWindowClosed;
    }

    public void RefreshBackdrop()
    {
        this.Backdrop = new MicaSystemBackdrop();
    }

    private async void OnWindowClosed(object sender, WindowEventArgs e)
    {
        bool firstTime = this.localSettingsService.ReadSettings<bool>(ILocalSettingsService.FirstTimeKey);
        bool minimize = this.localSettingsService.ReadSettings<bool>(ILocalSettingsService.MinimizeAppKey);

        // var dialog = new WindowCloseDialog();

        if (firstTime)
        {
            e.Handled = true;
            this.RefreshBackdrop();

            var dialog = new ContentDialog
            {
                Title = "Application closing",
                Content = "Do you want to minimize the application?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                CloseButtonText = "Cancel",
                XamlRoot = this.Content?.XamlRoot
            };

            var result = await dialog.ShowAsync();

            await localSettingsService.SaveSettingsAsync(ILocalSettingsService.FirstTimeKey, minimize);
        }

        if (minimize)
        {

        }
        else
        {
            App.MainWindow.Close();
        }
    }
}