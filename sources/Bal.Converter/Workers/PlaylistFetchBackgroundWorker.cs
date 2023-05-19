using AutoMapper;

using Bal.Converter.Common.Media;
using Bal.Converter.Messages;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.Services;
using Bal.Converter.YouTubeDl;

using CommunityToolkit.Mvvm.Messaging;

namespace Bal.Converter.Workers;

public class PlaylistFetchBackgroundWorker
{
    private readonly IDownloadsRegistryService downloadsRegistry;
    private readonly IYouTubeDl youtubeDl;
    private readonly IMapper mapper;

    public PlaylistFetchBackgroundWorker(IDownloadsRegistryService downloadsRegistry, IYouTubeDl youtubeDl, IMapper mapper)
    {
        this.downloadsRegistry = downloadsRegistry;
        this.youtubeDl = youtubeDl;
        this.mapper = mapper;
    }

    public async Task Process(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
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

                var download = new DownloadJob(video.Url)
                {
                    AutomaticQualityOption = job.AutomaticQualityOption,
                    TargetFormat = job.TargetFormat,
                    State = DownloadState.Pending,
                    Tags = tags
                };

                this.downloadsRegistry.EnqueueDownload(download);
                WeakReferenceMessenger.Default.Send(new DownloadAddedMessage(download));
            }
        }
    }
}