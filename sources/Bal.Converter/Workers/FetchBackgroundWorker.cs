using Bal.Converter.Common.Media;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.Services;
using Bal.Converter.YouTubeDl;

namespace Bal.Converter.Workers;

public class FetchBackgroundWorker
{
    private readonly IDownloadsRegistryService downloadsRegistry;
    private readonly IYouTubeDl youtubeDl;

    public FetchBackgroundWorker(IDownloadsRegistryService downloadsRegistry, IYouTubeDl youtubeDl)
    {
        this.downloadsRegistry = downloadsRegistry;
        this.youtubeDl = youtubeDl;
    }

    public async Task Process(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            var job = await this.downloadsRegistry.GetFetchJob();
            
            job.State = DownloadState.Fetching;

            var video = await this.youtubeDl.GetVideo(job.Url);

            var tags = new MediaTags
            {
                Title = video.Title,
                Artist = video.Channel,
                Year = video.UploadDate.Year
            };

            job.Tags = tags;

            this.downloadsRegistry.EnqueueDownload(job);
        }
    }
}