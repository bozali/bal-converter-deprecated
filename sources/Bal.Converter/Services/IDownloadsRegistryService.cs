using Bal.Converter.Modules.Downloads;

namespace Bal.Converter.Services ;

public interface IDownloadsRegistryService
{
    Task<DownloadJob> GetDownloadJob();

    void EnqueueFetch(string url);

    void EnqueueDownload();
}