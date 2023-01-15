using Bal.Converter.Modules.Downloads;

namespace Bal.Converter.Services ;

public class DownloadsRegistryService : IDownloadsRegistryService
{
    private readonly SemaphoreSlim semaphore;

    public DownloadsRegistryService()
    {
        this.semaphore = new SemaphoreSlim(0);
    }

    public async Task<DownloadJob> GetDownloadJob()
    {
        await this.semaphore.WaitAsync();
        return new DownloadJob();
    }

    public void EnqueueFetch(string url)
    {
    }

    public void EnqueueDownload()
    {
    }

    public void EnqueueDownload(string url)
    {
    }
}