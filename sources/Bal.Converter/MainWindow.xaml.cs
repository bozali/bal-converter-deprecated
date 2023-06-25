using Bal.Converter.Extensions;
using Bal.Converter.Messages;
using Bal.Converter.Services;

using CommunityToolkit.Mvvm.Messaging;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
        this.Title = "AppDisplayName".GetLocalized() + " Preview";

        this.Closed += this.OnWindowClosed;
    }

    public void RefreshBackdrop()
    {
        this.Backdrop = new MicaSystemBackdrop();
    }

    private async void OnWindowClosed(object sender, WindowEventArgs e)
    {
        bool firstTime = this.localSettingsService.ReadSettings<bool>(ILocalSettingsService.FirstTimeKey);
        bool minimize = this.localSettingsService.ReadSettings<bool>(ILocalSettingsService.MinimizeAppKey);

        if (firstTime && !App.ForceQuit)
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

            if (result == ContentDialogResult.None)
            {
                return;
            }

            minimize = result == ContentDialogResult.Primary;

            await this.localSettingsService.SaveSettingsAsync(ILocalSettingsService.FirstTimeKey, false);
        }

        await this.localSettingsService.SaveSettingsAsync(ILocalSettingsService.MinimizeAppKey, minimize);

        if (minimize)
        {
            App.MainWindow.Hide();
            WeakReferenceMessenger.Default.Send(new WindowStateChangedMessage(false));
        }
        else
        {
            App.MainWindow.Close();
        }
    }
}