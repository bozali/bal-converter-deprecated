using Bal.Converter.Events;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.Downloads.ViewModels;

public partial class DownloadJobViewModel : ObservableObject
{
    private readonly DownloadJob job;

    [ObservableProperty] private int id;
    [ObservableProperty] private string url;
    [ObservableProperty] private string title;
    [ObservableProperty] private string progress;
    [ObservableProperty] private DownloadState state;

    public DownloadJobViewModel(DownloadJob job)
    {
        this.job = job;

        this.Url = this.job.Url;
        this.State = this.job.State;
        this.Title = this.job.Tags.Title;

        this.job.StateChanged += OnDownloadStateChanged;
    }

    private void OnDownloadStateChanged(object sender, DownloadStateChangedEventArgs e)
    {
        if (e.State == DownloadState.Pending || e.State == DownloadState.Pending)
        {
            this.Title = this.job.Tags.Title;
        }
    }
}