using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class FormatViewModel : ObservableObject
{
    [ObservableProperty] private string resolution;
    [ObservableProperty] private string extension;
    [ObservableProperty] private string vcodec;
    [ObservableProperty] private string acodec;
    [ObservableProperty] private string filesize;
    [ObservableProperty] private bool isAudioOnly;
    [ObservableProperty] private bool isVideoOnly;
    [ObservableProperty] private bool isVideoAndAudio;
}