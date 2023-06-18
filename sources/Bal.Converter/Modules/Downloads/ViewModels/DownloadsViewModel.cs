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

        this.DownloadJobs = new ObservableCollection<DownloadJobViewModel>(this.downloadsRegistry.AllJobs.Select(x => new DownloadJobViewModel(this.downloadsRegistry, x)));

        WeakReferenceMessenger.Default.Register<DownloadRemovedMessage>(this, this.OnDownloadRemoved);
        WeakReferenceMessenger.Default.Register<DownloadAddedMessage>(this, this.OnDownloadAdded);
    }

    public ObservableCollection<DownloadJobViewModel> DownloadJobs { get; set; }

    [RelayCommand]
    private void Clear()
    {
        var toRemove = new List<int>();

        for (int i = 0; i < this.DownloadJobs.Count; i++)
        {
            var item = this.DownloadJobs[i];

            if (item.State != DownloadState.Done && item.State != DownloadState.Cancelled)
            {
                continue;
            }

            toRemove.Add(item.Id);
        }

        foreach (int id in toRemove)
        {
            this.downloadsRegistry.Remove(id);
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

    private void OnDownloadAdded(object recipient, DownloadAddedMessage message)
    {
        this.DownloadJobs.Add(new DownloadJobViewModel(this.downloadsRegistry, message.Value));
    }
}