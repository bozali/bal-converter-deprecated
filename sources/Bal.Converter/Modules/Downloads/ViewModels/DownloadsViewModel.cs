using System.Collections.ObjectModel;

using Bal.Converter.Services;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.Downloads.ViewModels;

public class DownloadsViewModel : ObservableObject
{
    private readonly IDownloadsRegistryService downloadsRegistry;

    public DownloadsViewModel(IDownloadsRegistryService downloadsRegistry)
    {
        this.downloadsRegistry = downloadsRegistry;

        this.DownloadJobs = new ObservableCollection<DownloadJobViewModel>(this.downloadsRegistry.AllJobs.Select(x => new DownloadJobViewModel(x)));
    }

    public ObservableCollection<DownloadJobViewModel> DownloadJobs { get; set; }
}