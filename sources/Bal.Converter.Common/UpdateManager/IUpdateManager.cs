namespace Bal.Converter.Common.Updater;

public interface IUpdateManager
{
    Task DownloadUpdate();

    bool HasNewVersion();
}