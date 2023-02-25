using System.Web;

using Bal.Converter.Common.Enums;
using Bal.Converter.Common.Extensions;
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
                                    IYouTubeDl youtubeDl)
    {
        this.logger = logger;
        this.navigationService = navigationService;
        this.fileDownloader = fileDownloader;
        this.downloadsRegistry = downloadsRegistry;
        this.youtubeDl = youtubeDl;
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
            this.downloadsRegistry.EnqueueFetch(this.Url, Enum.Parse<MediaFileExtension>(this.Format), new QualityOption { AudioQuality = Enum.Parse<AutomaticQualityOption>(this.AudioQualityOption), VideoQuality = Enum.Parse<AutomaticQualityOption>(this.VideoQualityOption) });
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

            var video = await this.youtubeDl.GetVideo(this.Url);
            var thumbnail = await this.fileDownloader.DownloadFileAsync(video.ThumbnailUrl);

            var vmVideo = new VideoViewModel
            {
                Format = this.Format,
                Url = video.Url,
                ThumbnailData = thumbnail.Data,
                ThumbnailPath = thumbnail.DownloadPath,
                Tags = new MediaTagsViewModel
                {
                    Title = video.Title.RemoveIllegalChars(),
                    Artist = video.Channel,
                    Year = video.UploadDate.Year
                }
            };

            var parameters = new Dictionary<string, object>
            {
                { "Video", vmVideo },
                { "VideoQuality", this.VideoQualityOption },
                { "AudioQuality", this.AudioQualityOption }
            };

            this.navigationService.NavigateTo(typeof(MediaTagEditorViewModel).FullName!, parameters);
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
            this.IsPlaylist = true;
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