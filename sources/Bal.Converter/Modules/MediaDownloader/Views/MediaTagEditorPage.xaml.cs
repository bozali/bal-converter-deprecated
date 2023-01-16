using Bal.Converter.Modules.MediaDownloader.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Bal.Converter.Modules.MediaDownloader.Views;

public sealed partial class MediaTagEditorPage : Page
{
    public MediaTagEditorPage()
    {
        this.ViewModel = App.GetService<MediaTagEditorViewModel>();

        this.InitializeComponent();
    }

    public MediaTagEditorViewModel ViewModel { get; }
}
