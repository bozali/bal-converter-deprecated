using Bal.Converter.Common.Enums;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.YouTubeDl.Quality;

namespace Bal.Converter.Services ;

public interface IDownloadsRegistryService
{
    IEnumerable<DownloadJob> GetDownloadJobs();

    Task<DownloadJob> GetDownloadJob();

    Task<DownloadJob> GetFetchJob();

    void EnqueueFetch(string url, MediaFileExtension format, QualityOption option);

    void EnqueueDownload(DownloadJob job);
}