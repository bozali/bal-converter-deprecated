using Bal.Converter.Modules.Downloads.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Bal.Converter.Modules.Downloads.Views;

public sealed partial class DownloadsPage : Page
{
    public DownloadsPage()
    {
        this.ViewModel = App.GetService<DownloadsViewModel>();

        this.InitializeComponent();
    }

    public DownloadsViewModel ViewModel { get; }
}
