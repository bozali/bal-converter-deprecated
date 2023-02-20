using Bal.Converter.Common.Enums;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.YouTubeDl.Quality;

namespace Bal.Converter.Services ;

public interface IDownloadsRegistryService
{
    IReadOnlyCollection<DownloadJob> AllJobs { get; }

    Task<DownloadJob?> NextDownloadJob();

    Task<DownloadJob?> NextFetchJob();

    void EnqueueFetch(string url, MediaFileExtension format, QualityOption option);

    void EnqueueDownload(DownloadJob job);

    void UpdateState(DownloadJob job, DownloadState state);

    void Remove(int id);
}