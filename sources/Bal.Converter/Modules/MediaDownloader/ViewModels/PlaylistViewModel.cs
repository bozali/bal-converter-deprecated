using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class PlaylistViewModel : ObservableObject
{
    [ObservableProperty] private string id;
    [ObservableProperty] private string title;
    [ObservableProperty] private int playlistCount;
    [ObservableProperty] private string url;
    [ObservableProperty] private ObservableCollection<VideoViewModel> videos;
}