using Bal.Converter.Domain;

namespace Bal.Converter.Services;

public interface IFileDownloaderService
{
    Task<FileDownloadResponse> DownloadFileAsync(string url);
}