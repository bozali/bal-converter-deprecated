using Bal.Converter.Common.Media;
using Bal.Converter.Common.Web;
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
            try
            {
                var job = await this.downloadsRegistry.NextFetchJob();

                if (job == null)
                {
                    continue;
                }

                job.State = DownloadState.Fetching;

                var video = await this.youtubeDl.GetVideo(job.Url);

                var downloadResponse = await this.fileDownloaderService.DownloadImageAsync(video.ThumbnailUrl, Path.Combine(ILocalSettingsService.TempPath, "Thumbnails", Guid.NewGuid() + ".jpg"));
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
            catch (Exception e)
            {
            }
        }
    }
}