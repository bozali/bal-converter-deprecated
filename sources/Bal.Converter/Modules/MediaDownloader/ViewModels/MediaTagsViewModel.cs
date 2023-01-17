using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class MediaTagsViewModel : ObservableObject
{
    [ObservableProperty] private string title;
    [ObservableProperty] private string artist;
    [ObservableProperty] private string album;
    [ObservableProperty] private string comment;
    [ObservableProperty] private string copyright;
    [ObservableProperty] private string albumArtists;
    [ObservableProperty] private string composers;
    [ObservableProperty] private string genres;
    [ObservableProperty] private string performers;
    [ObservableProperty] private int year;
}