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
        // TODO Investigate why ApplicatinData is not working
        // get => RuntimeHelper.IsMsix ? ApplicationData.Current.TemporaryFolder.Path : Path.Combine(Path.GetTempPath(), "BalConverter");
        get => Path.Combine(Path.GetTempPath(), "BalConverter");
    }

    static string AppDataPath
    {
        get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BalConverter");
    }

    Task<T?> ReadSettingsAsync<T>(string key);

    T? ReadSettings<T>(string key);

    Task SaveSettingsAsync<T>(string key, T value);

    void SaveSettings<T>(string key, T value);
}