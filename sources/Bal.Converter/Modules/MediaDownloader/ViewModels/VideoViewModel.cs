using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class VideoViewModel : ObservableObject
{
    [ObservableProperty] private MediaTagsViewModel tags;
    [ObservableProperty] private string title;
    [ObservableProperty] private string url;
    [ObservableProperty] private string format;
    [ObservableProperty] private bool isSelected;
    [ObservableProperty] private string thumbnailPath;

    [ObservableProperty] private FormatViewModel[] formats;
}