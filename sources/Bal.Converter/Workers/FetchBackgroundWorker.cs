using Bal.Converter.Common.Media;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.Services;
using Bal.Converter.YouTubeDl;

namespace Bal.Converter.Workers;

public class FetchBackgroundWorker
{
    private readonly IDownloadsRegistryService downloadsRegistry;
    private readonly IFileDownloaderService fileDownloaderService;
    private readonly IYouTubeDl youtubeDl;

    public FetchBackgroundWorker(IDownloadsRegistryService downloadsRegistry, IFileDownloaderService fileDownloaderService, IYouTubeDl youtubeDl)
    {
        this.downloadsRegistry = downloadsRegistry;
        this.fileDownloaderService = fileDownloaderService;
        this.youtubeDl = youtubeDl;
    }

    public async Task Process(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            var job = await this.downloadsRegistry.NextFetchJob();
            
            job.State = DownloadState.Fetching;

            var video = await this.youtubeDl.GetVideo(job.Url);

            var downloadResponse = await this.fileDownloaderService.DownloadFileAsync(video.ThumbnailUrl);
            job.ThumbnailPath = downloadResponse.DownloadPath;
            
            var tags = new MediaTags
            {
                Title = video.Title,
                Artist = video.Channel,
                Year = video.UploadDate.Year
            };

            job.Tags = tags;

            this.downloadsRegistry.UpdateState(job, DownloadState.Downloading);
        }
    }
}