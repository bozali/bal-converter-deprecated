using System.Collections.ObjectModel;

using Bal.Converter.Messages;
using Bal.Converter.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bal.Converter.Modules.Downloads.ViewModels;

public partial class DownloadsViewModel : ObservableObject
{
    private readonly IDownloadsRegistryService downloadsRegistry;

    public DownloadsViewModel(IDownloadsRegistryService downloadsRegistry)
    {
        this.downloadsRegistry = downloadsRegistry;

        this.DownloadJobs = new ObservableCollection<DownloadJobViewModel>(this.downloadsRegistry.AllJobs.Select(x => new DownloadJobViewModel(x)));
        WeakReferenceMessenger.Default.Register<DownloadRemovedMessage>(this, this.OnDownloadRemoved);
    }

    public ObservableCollection<DownloadJobViewModel> DownloadJobs { get; set; }

    [RelayCommand]
    private void Clear()
    {
        for (int i = 0; i < this.DownloadJobs.Count; i++)
        {
            var item = this.DownloadJobs[i];

            if (item.State != DownloadState.Done)
            {
                continue;
            }

            this.downloadsRegistry.Remove(item.Id);
        }
    }

    private void OnDownloadRemoved(object recipient, DownloadRemovedMessage message)
    {
        var item = this.DownloadJobs.FirstOrDefault(x => x.Id == message.Value);

        if (item != null)
        {
            this.DownloadJobs.Remove(item);
        }
    }
}