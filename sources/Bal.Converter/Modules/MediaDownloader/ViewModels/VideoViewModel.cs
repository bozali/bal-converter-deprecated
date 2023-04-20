using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class VideoViewModel : ObservableObject
{
    [ObservableProperty] private MediaTagsViewModel tags;
    [ObservableProperty] private string title;
    [ObservableProperty] private string url;
    [ObservableProperty] private string format;
    [ObservableProperty] private bool isSelected;
    [ObservableProperty] private string thumbnailPath;
}