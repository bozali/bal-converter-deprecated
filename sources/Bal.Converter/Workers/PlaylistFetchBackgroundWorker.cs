using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using AutoMapper;

using Bal.Converter.Common.Media;
using Bal.Converter.Common.Web;
using Bal.Converter.Messages;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.Services;
using Bal.Converter.YouTubeDl;
using CommunityToolkit.Mvvm.Messaging;

namespace Bal.Converter.Workers;

public class PlaylistFetchBackgroundWorker
{
    private static readonly DispatcherQueue MyDispatcher = DispatcherQueue.GetForCurrentThread();

    private readonly IDownloadsRegistryService downloadsRegistry;
    private readonly IFileDownloaderService fileDownloaderService;
    private readonly IYouTubeDl youtubeDl;
    private readonly IMapper mapper;

    public PlaylistFetchBackgroundWorker(IDownloadsRegistryService downloadsRegistry, IFileDownloaderService fileDownloaderService, IYouTubeDl youtubeDl, IMapper mapper)
    {
        this.downloadsRegistry = downloadsRegistry;
        this.fileDownloaderService = fileDownloaderService;
        this.youtubeDl = youtubeDl;
        this.mapper = mapper;
    }

    public async Task Process(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var job = await this.downloadsRegistry.NextPlaylistFetchJob().ConfigureAwait(false);

                if (job == null)
                {
                    continue;
                }

                var playlist = await this.youtubeDl.GetPlaylist(job.Url).ConfigureAwait(false);

                foreach (var video in playlist.Videos)
                {
                    var tags = new MediaTags
                    {
                        Title = video.Title,
                        Artist = video.Channel,
                        Year = video.UploadDate.Year
                    };

                    var downloadResponse = await this.fileDownloaderService.DownloadImageAsync(video.ThumbnailUrl, Path.Combine(ILocalSettingsService.TempPath, "Thumbnails", Guid.NewGuid() + ".jpg"));

                    var download = new DownloadJob(video.Url)
                    {
                        AutomaticQualityOption = job.AutomaticQualityOption,
                        ThumbnailPath = downloadResponse.DownloadPath,
                        TargetFormat = job.TargetFormat,
                        State = DownloadState.Pending,
                        Tags = tags,
                    };

                    // var dispatcher = Windows.Threading.Dispatcher.FromThread(Thread.CurrentThread);

                    this.downloadsRegistry.EnqueueDownload(download);
                    
                    WeakReferenceMessenger.Default.Send(new DownloadAddedMessage(download));
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}