using Bal.Converter.Common.Extensions;
using Bal.Converter.Common.Web;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.YouTubeDl;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Dispatching;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class PlaylistOverviewViewModel : ObservableObject, INavigationAware
{
    private readonly IFileDownloaderService fileDownloaderService;

    [ObservableProperty] private Playlist playlist;
    [ObservableProperty] private string videoQualityOption;
    [ObservableProperty] private string audioQualityOption;

    public PlaylistOverviewViewModel(IFileDownloaderService fileDownloaderService)
    {
        this.fileDownloaderService = fileDownloaderService;
    }
    
    public void OnNavigatedTo(object parameter)
    {
        var input = (IDictionary<string, object>)parameter;

        this.Playlist = input.Get<Playlist>("Playlist")!;
        this.AudioQualityOption = input.Get<string>("AudioQuality")!;
        this.VideoQualityOption = input.Get<string>("VideoQuality")!;

        this.DownloadThumbnailsAsync().ConfigureAwait(false);
    }

    public void OnNavigatedFrom()
    {
    }

    private async Task DownloadThumbnailsAsync()
    {
        foreach (var video in this.Playlist.Videos)
        {
            // video.ThumbnailData = await this.fileDownloaderService.DownloadImageDataAsync(video.ThumbnailUrl);
        }
    }
}