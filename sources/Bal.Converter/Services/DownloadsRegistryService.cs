using Bal.Converter.Common.Enums;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.YouTubeDl.Quality;

namespace Bal.Converter.Services ;

public class DownloadsRegistryService : IDownloadsRegistryService
{
    private readonly SemaphoreSlim semaphore;
    private readonly SemaphoreSlim fetchSemaphore;
    private readonly List<DownloadJob> fetchJobs;
    private readonly List<DownloadJob> downloadJobs;

    public DownloadsRegistryService()
    {
        this.fetchSemaphore = new SemaphoreSlim(0);
        this.semaphore = new SemaphoreSlim(0);

        this.downloadJobs = new List<DownloadJob>();
        this.fetchJobs = new List<DownloadJob>();
    }

    public IEnumerable<DownloadJob> GetDownloadJobs()
    {
        return this.downloadJobs.Concat(this.fetchJobs);
    }

    public async Task<DownloadJob> GetDownloadJob()
    {
        await this.semaphore.WaitAsync();
        return this.downloadJobs.First();
    }

    public async Task<DownloadJob> GetFetch()
    {
        await this.fetchSemaphore.WaitAsync();
        return this.fetchJobs.First();
    }

    public void EnqueueFetch(string url, MediaFileExtension format, QualityOption quality)
    {
        var job = new DownloadJob
        {
            Url = url,
            TargetFormat = format,
            AutomaticQualityOption = quality
        };

        this.fetchJobs.Add(job);
        this.fetchSemaphore.Release();
    }

    public void EnqueueDownload()
    {
    }
}