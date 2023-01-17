using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class VideoViewModel : ObservableObject
{
    [ObservableProperty] private MediaTagsViewModel tags;
    [ObservableProperty] private byte[] thumbnailData;
    [ObservableProperty] private string url;
    [ObservableProperty] private string format;
    [ObservableProperty] private bool isSelected;
    [ObservableProperty] private string thumbnailPath;
}