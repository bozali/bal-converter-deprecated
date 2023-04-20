namespace Bal.Converter.Common.Web;

public interface IFileDownloaderService
{
    Task<FileDownloadResponse> DownloadImageAsync(string url, string path);

    Task<byte[]> DownloadImageDataAsync(string url);
}