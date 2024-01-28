namespace Bal.Converter.Common.Updater;

public class FFmpegUpdateManager : IUpdateManager
{
    public Task DownloadUpdate()
    {
        return Task.CompletedTask;
    }

    public bool HasNewVersion()
    {
        // TODO Check https://github.com/BtbN/FFmpeg-Builds/releases

        return true;
    }
}