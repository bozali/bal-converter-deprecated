using System.Collections.ObjectModel;

using AutoMapper;

using Bal.Converter.Common.Extensions;
using Bal.Converter.Common.Web;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.Services;
using Bal.Converter.YouTubeDl;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class PlaylistOverviewViewModel : ObservableObject, INavigationAware
{
    private readonly IFileDownloaderService fileDownloaderService;
    private readonly IMapper mapper;

    [ObservableProperty] private PlaylistViewModel playlist;
    [ObservableProperty] private string videoQualityOption;
    [ObservableProperty] private string audioQualityOption;
    [ObservableProperty] private bool isProcessing;

    public PlaylistOverviewViewModel(IFileDownloaderService fileDownloaderService, IMapper mapper)
    {
        this.fileDownloaderService = fileDownloaderService;
        this.mapper = mapper;
    }

    public Playlist Model { get; set; }

    public string DefaultFormat { get; set; }

    public void OnNavigatedTo(object parameter)
    {
        var input = (IDictionary<string, object>)parameter;

        this.Model = input.Get<Playlist>("Playlist")!;
        this.DefaultFormat = input.Get<string>("Format")!;
        this.AudioQualityOption = input.Get<string>("AudioQuality")!;
        this.VideoQualityOption = input.Get<string>("VideoQuality")!;

        this.Playlist = this.mapper.Map<PlaylistViewModel>(this.Model);
        this.Playlist.Videos = new ObservableCollection<VideoViewModel>();

        this.IsProcessing = true;

        this.MapDataViewModelsAsync().ConfigureAwait(false);
    }

    public void OnNavigatedFrom()
    {
    }

    private async Task MapDataViewModelsAsync()
    {
        foreach (var video in this.Model.Videos)
        {
            string path = Path.Combine(ILocalSettingsService.TempPath, "Thumbnails", this.Playlist.Id, Guid.NewGuid() + ".jpg");
            var response = await this.fileDownloaderService.DownloadImageAsync(video.ThumbnailUrl, path);

            var item = this.mapper.Map<VideoViewModel>(video);
            item.ThumbnailPath = response.DownloadPath;
            item.Format = this.DefaultFormat;

            this.Playlist.Videos.Add(item);
        }

        this.IsProcessing = false;
    }
}