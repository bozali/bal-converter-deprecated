using System.Collections.ObjectModel;
using System.Drawing;
using ABI.Microsoft.Web.WebView2.Core;
using Bal.Converter.Common.Extensions;
using Bal.Converter.Common.Web;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.Services;
using Bal.Converter.YouTubeDl;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class PlaylistOverviewViewModel : ObservableObject, INavigationAware
{
    private readonly IFileDownloaderService fileDownloaderService;

    [ObservableProperty] private PlaylistViewModel playlist;
    [ObservableProperty] private string videoQualityOption;
    [ObservableProperty] private string audioQualityOption;

    private Playlist playlistModel;
    private string format;

    public PlaylistOverviewViewModel(IFileDownloaderService fileDownloaderService)
    {
        this.fileDownloaderService = fileDownloaderService;
    }
    
    public void OnNavigatedTo(object parameter)
    {
        var input = (IDictionary<string, object>)parameter;

        this.playlistModel = input.Get<Playlist>("Playlist")!;
        this.format = input.Get<string>("Format")!;
        this.AudioQualityOption = input.Get<string>("AudioQuality")!;
        this.VideoQualityOption = input.Get<string>("VideoQuality")!;

        this.Playlist = new PlaylistViewModel
        {
            Id = this.playlistModel.Id,
            Url = this.playlistModel.Url,
            Title = this.playlistModel.Title,
            PlaylistCount = this.playlistModel.PlaylistCount,
            Videos = new ObservableCollection<VideoViewModel>()
        };

        this.DownloadThumbnailsAsync().ConfigureAwait(false);
    }

    public void OnNavigatedFrom()
    {
    }

    private async Task DownloadThumbnailsAsync()
    {
        foreach (var item in this.playlistModel.Videos.Select((value, index) => new { Index = index, Value = value }))
        {
            string path = Path.Combine(ILocalSettingsService.TempPath, "Thumbnails", this.Playlist.Id, Guid.NewGuid() + ".jpg");
            var response = await this.fileDownloaderService.DownloadImageAsync(item.Value.ThumbnailUrl, path);

            var video = new VideoViewModel
            {
                Format = this.format,
                Title = item.Value.Title,
                Url = item.Value.Url,
                ThumbnailPath = response.DownloadPath,
                Tags = new MediaTagsViewModel
                {
                    Title = item.Value.Title.RemoveIllegalChars(),
                    Artist = item.Value.Channel,
                    Year = item.Value.UploadDate.Year
                },
            };

            this.Playlist.Videos.Add(video);
        }
    }
}