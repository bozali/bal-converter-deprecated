using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.Downloads.ViewModels;

public class DownloadsViewModel : ObservableObject
{
    public DownloadsViewModel()
    {
        this.DownloadJobs = new ObservableCollection<DownloadJobViewModel>
        {
            new DownloadJobViewModel { Id = 0, Url = "http://www.youtube.com/", Title = "Marcus Warner - 39 Seconds" },
            new DownloadJobViewModel { Id = 0, Url = "http://www.youtube.com/", Title = "Marcus Warner - 39 Seconds" },
            new DownloadJobViewModel { Id = 0, Url = "http://www.youtube.com/", Title = "Marcus Warner - 39 Seconds" },
            new DownloadJobViewModel { Id = 0, Url = "http://www.youtube.com/", Title = "Marcus Warner - 39 Seconds" },
        };
    }

    public ObservableCollection<DownloadJobViewModel> DownloadJobs { get; set; }
}