using Microsoft.UI.Xaml.Controls;

using Bal.Converter.Modules.MediaDownloader.ViewModels;
using Microsoft.UI.Xaml.Media;

namespace Bal.Converter.Modules.MediaDownloader.Views;

public sealed partial class MediaDownloaderPage : Page
{
    public MediaDownloaderPage()
    {
        this.ViewModel = App.GetService<MediaDownloaderViewModel>();

        this.InitializeComponent();
    }

    public MediaDownloaderViewModel ViewModel { get; }
}
