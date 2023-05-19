using Windows.Storage;

using Bal.Converter.Common.Services;
using Bal.Converter.Helpers;
using Bal.Converter.Modules.Settings;

using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Bal.Converter.Services ;

public class LocalSettingsService : ILocalSettingsService
{
    private readonly IFileSystemService filesystemService;
    private const string DefaultApplicationDataFolder = "Bal-Converter/ApplicationData";
    private const string DefaultLocalSettingsFile = "LocalSettings.json";

    private readonly LocalSettingsOptions options;

    private readonly string localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string applicationDataFolder;
    private readonly string localSettingsFile;

    private IDictionary<string, object> settings;
    private bool isInitialized;

    public LocalSettingsService(IFileSystemService filesystemService, IOptions<LocalSettingsOptions> options)
    {
        this.filesystemService = filesystemService;
        this.options = options.Value;

        this.applicationDataFolder = Path.Combine(this.localApplicationData, this.options.ApplicationDataFolder ?? DefaultApplicationDataFolder);
        this.localSettingsFile = this.options.LocalSettingsFile ?? DefaultLocalSettingsFile;

        this.settings = new Dictionary<string, object>();
    }

    public async Task<T?> ReadSettingsAsync<T>(string key)
    {
        return await Task.Run(() => this.ReadSettings<T>(key));
    }

    public T? ReadSettings<T>(string key)
    {
        if (RuntimeHelper.IsMsix)
        {
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out object? obj))
            {
                return JsonConvert.DeserializeObject<T>((string)obj);
            }
        }
        else
        {
            this.Initialize();

            if (this.settings.TryGetValue(key, out object? obj))
            {
                return JsonConvert.DeserializeObject<T>((string)obj);
            }
        }

        return default;
    }

    public async Task SaveSettingsAsync<T>(string key, T value)
    {
        await Task.Run(() => this.SaveSettings(key, value));
    }

    public void SaveSettings<T>(string key, T value)
    {
        if (RuntimeHelper.IsMsix)
        {
            ApplicationData.Current.LocalSettings.Values[key] = JsonConvert.SerializeObject(value);
        }
        else
        {
            this.Initialize();

            this.settings[key] = JsonConvert.SerializeObject(value);

            this.filesystemService.WriteJson(Path.Combine(this.applicationDataFolder, this.localSettingsFile), this.settings);
        }
    }

    private void Initialize()
    {
        if (this.isInitialized)
        {
            this.settings = this.filesystemService.ReadJson<IDictionary<string, object>>(Path.Combine(this.applicationDataFolder, this.localSettingsFile)) ?? new Dictionary<string, object>();
            this.isInitialized = true;
        }
    }
}