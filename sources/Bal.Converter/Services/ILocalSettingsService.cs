using Windows.Storage;

using Bal.Converter.Helpers;

namespace Bal.Converter.Services ;

public interface ILocalSettingsService
{
    const string DownloadDirectoryKey = "DownloadDirectory";
    const string FirstTimeKey = "AppStartedFirstTime";
    const string MinimizeAppKey = "MinimizeApp";
    const string BandwidthKey = "Bandwidth";
    const string BandwidthMinimizedKey = "BandwidthMinimized";

    static string TempPath
    {
        get => RuntimeHelper.IsMsix ? ApplicationData.Current.TemporaryFolder.Path : Path.Combine(Path.GetTempPath(), "BalConverter"); }

    Task<T?> ReadSettingsAsync<T>(string key);

    T? ReadSettings<T>(string key);

    Task SaveSettingsAsync<T>(string key, T value);

    void SaveSettings<T>(string key, T value);
}