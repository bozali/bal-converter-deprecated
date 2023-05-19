using System.Web;

using AutoMapper;

using Bal.Converter.Common.Enums;
using Bal.Converter.Common.Web;
using Bal.Converter.Contracts.Services;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.Messages;
using Bal.Converter.Services;
using Bal.Converter.YouTubeDl;
using Bal.Converter.YouTubeDl.Quality;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class MediaDownloaderViewModel : ObservableObject, INavigationAware
{
    private readonly ILogger<MediaDownloaderViewModel> logger;
    private readonly INavigationService navigationService;
    private readonly IFileDownloaderService fileDownloader;
    private readonly IDownloadsRegistryService downloadsRegistry;
    private readonly IYouTubeDl youtubeDl;
    private readonly IMapper mapper;

    [ObservableProperty] private string audioQualityOption;
    [ObservableProperty] private string videoQualityOption;
    [ObservableProperty] private bool proceedAsPlaylist;
    [ObservableProperty] private bool isPlaylist;
    [ObservableProperty] private bool isProcessing;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPlaylist))]
    [NotifyCanExecuteChangedFor(nameof(EditCommand))]
    [NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
    public string url;

    private string format;

    public MediaDownloaderViewModel(ILogger<MediaDownloaderViewModel> logger,
                                    INavigationService navigationService,
                                    IFileDownloaderService fileDownloader,
                                    IDownloadsRegistryService downloadsRegistry,
                                    IYouTubeDl youtubeDl,
                                    IMapper mapper)
    {
        this.logger = logger;
        this.navigationService = navigationService;
        this.fileDownloader = fileDownloader;
        this.downloadsRegistry = downloadsRegistry;
        this.youtubeDl = youtubeDl;
        this.mapper = mapper;

        this.Url = string.Empty;

        this.AudioQualityOption = AutomaticQualityOption.Best.ToString();
        this.VideoQualityOption = AutomaticQualityOption.Best.ToString();
        this.Format = MediaFileExtension.MP4.ToString();
        this.ProceedAsPlaylist = true;
        this.IsProcessing = false;
        this.IsPlaylist = true;
    }

    public string Format
    {
        get => this.format;
        set => this.SetProperty(ref this.format, value);
    }

    public void OnNavigatedTo(object parameter)
    {
        this.Url = string.Empty;
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand(CanExecute = nameof(CanContinue))]
    private void Convert()
    {
        try
        {
            var option = new QualityOption
            {
                AudioQuality = Enum.Parse<AutomaticQualityOption>(this.AudioQualityOption),
                VideoQuality = Enum.Parse<AutomaticQualityOption>(this.VideoQualityOption)
            };

            // ReSharper disable once LocalVariableHidesMember
            var format = Enum.Parse<MediaFileExtension>(this.Format);

            if (this.IsPlaylist && this.ProceedAsPlaylist)
            {
                this.downloadsRegistry.EnqueuePlaylist(this.Url, format, option);
            }
            else
            {
                this.downloadsRegistry.EnqueueFetch(this.Url, format, option);
            }
        }
        catch (Exception e)
        {
            this.logger.LogError(e, $"Failed to convert {this.Url}");
        }
        finally
        {
            this.Url = string.Empty;
        }
    }

    [RelayCommand(CanExecute = nameof(CanContinue))]
    private async Task Edit()
    {
        try
        {
            this.IsProcessing = true;

            if (this.IsPlaylist && this.ProceedAsPlaylist)
            {
                var playlist = await this.youtubeDl.GetPlaylist(this.Url);

                var parameters = new Dictionary<string, object>
                {
                    { "Playlist", playlist },
                    { "Format", this.Format },
                    { "VideoQuality", this.VideoQualityOption },
                    { "AudioQuality", this.AudioQualityOption }
                };

                this.navigationService.NavigateTo(typeof(PlaylistOverviewViewModel).FullName!, parameters);
            }
            else
            {
                var video = await this.youtubeDl.GetVideo(this.Url);
                var thumbnail = await this.fileDownloader.DownloadImageAsync(video.ThumbnailUrl, Path.Combine(ILocalSettingsService.TempPath, "Thumbnails", Guid.NewGuid() + ".jpg"));

                var vm = new VideoViewModel
                {
                    Title = video.Title,
                    ThumbnailPath = thumbnail.DownloadPath,
                    Format = this.Format,
                    Tags = new MediaTagsViewModel
                    {
                        Title = video.Title,
                        Artist = video.Channel,
                        Year = video.UploadDate.Year
                    }
                };

                var parameters = new Dictionary<string, object>
                {
                    { "Video", vm },
                    { "VideoQuality", this.VideoQualityOption },
                    { "AudioQuality", this.AudioQualityOption }
                };

                this.navigationService.NavigateTo(typeof(MediaTagEditorViewModel).FullName!, parameters);
            }
        }
        catch (Exception e)
        {
            // ignored
        }
        finally
        {
            this.IsProcessing = false;
        }
    }

    private bool CanContinue()
    {
        return !string.IsNullOrEmpty(this.Url);
    }

    partial void OnUrlChanged(string value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            this.IsPlaylist = false;
            return;
        }

        this.IsPlaylist = HttpUtility.ParseQueryString(uri.Query)
                                     .AllKeys
                                     .Any(key => string.Equals(key, "list", StringComparison.OrdinalIgnoreCase));
    }

    partial void OnIsProcessingChanged(bool value)
    {
        WeakReferenceMessenger.Default.Send(new DisableInteractionChangeMessage(value));
    }
}