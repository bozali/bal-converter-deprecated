using Bal.Converter.Services;

namespace Bal.Converter.Workers;

public class FetchBackgroundWorker
{
    private readonly IDownloadsRegistryService downloadsRegistry;

    public FetchBackgroundWorker(IDownloadsRegistryService downloadsRegistry)
    {
        this.downloadsRegistry = downloadsRegistry;
    }

    public async Task Process(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            await this.downloadsRegistry.GetDownloadJob();
        }
    }
}