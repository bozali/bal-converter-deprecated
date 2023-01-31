using System.Collections.ObjectModel;

using AutoMapper;

using Bal.Converter.Services;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.Downloads.ViewModels;

public class DownloadsViewModel : ObservableObject
{
    private readonly IDownloadsRegistryService downloadsRegistry;
    private readonly IMapper mapper;

    public DownloadsViewModel(IDownloadsRegistryService downloadsRegistry, IMapper mapper)
    {
        this.downloadsRegistry = downloadsRegistry;
        this.mapper = mapper;

        this.DownloadJobs = new ObservableCollection<DownloadJobViewModel>(this.downloadsRegistry.GetDownloadJobs().Select(x => new DownloadJobViewModel(x)));
    }

    public ObservableCollection<DownloadJobViewModel> DownloadJobs { get; set; }
}