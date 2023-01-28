using Bal.Converter.Common.Enums;
using Bal.Converter.Modules.Downloads;

namespace Bal.Converter.Services ;

public interface IDownloadsRegistryService
{
    Task<DownloadJob> GetDownloadJob();

    Task<DownloadJob> GetFetch();

    void EnqueueFetch(string url, MediaFileExtension extension);

    void EnqueueDownload();
}