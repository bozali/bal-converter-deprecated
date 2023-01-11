using Bal.Converter.Contracts.Services;
using Bal.Converter.Modules.MediaDownloader.ViewModels;
using Bal.Converter.Modules.MediaDownloader.Views;
using Bal.Converter.Modules.Settings.ViewModels;
using Bal.Converter.Modules.Settings.Views;
using Bal.Converter.ViewModels;
using Bal.Converter.Views;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

namespace Bal.Converter.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> pages = new();

    public PageService()
    {
        this.Configure<MediaDownloaderViewModel, MediaDownloaderPage>();
        this.Configure<MainViewModel, MainPage>();
        this.Configure<SettingsViewModel, SettingsPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (pages)
        {
            if (!pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<TVm, TV>() where TVm : ObservableObject where TV : Page
    {
        lock (pages)
        {
            var key = typeof(TVm).FullName!;
            if (pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(TV);
            if (pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {pages.First(p => p.Value == type).Key}");
            }

            pages.Add(key, type);
        }
    }
}
