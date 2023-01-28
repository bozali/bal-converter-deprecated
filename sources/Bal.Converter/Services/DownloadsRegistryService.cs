using Bal.Converter.Common.Enums;
using Bal.Converter.Modules.Downloads;

namespace Bal.Converter.Services ;

public class DownloadsRegistryService : IDownloadsRegistryService
{
    private readonly SemaphoreSlim semaphore;
    private readonly SemaphoreSlim fetchSemaphore;

    public DownloadsRegistryService()
    {
        this.fetchSemaphore = new SemaphoreSlim(0);
        this.semaphore = new SemaphoreSlim(0);
    }

    public async Task<DownloadJob> GetDownloadJob()
    {
        await this.semaphore.WaitAsync();
        return new DownloadJob();
    }

    public async Task<DownloadJob> GetFetch()
    {
        await this.fetchSemaphore.WaitAsync();
        return new DownloadJob();
    }

    public void EnqueueFetch(string url, MediaFileExtension extension)
    {
    }

    public void EnqueueDownload()
    {
    }

    public void EnqueueDownload(string url)
    {
    }
}