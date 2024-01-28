namespace Bal.Converter.UpdateManager.YouTubeDl;

public interface IYouTubeDlUpdateManager
{
    Task DownloadUpdate();

    Task<bool> HasNewVersion();
}