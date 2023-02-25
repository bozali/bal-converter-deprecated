namespace Bal.Converter.Common.Web;

public interface IFileDownloaderService
{
    Task<FileDownloadResponse> DownloadImageAsync(string url, string path);
}