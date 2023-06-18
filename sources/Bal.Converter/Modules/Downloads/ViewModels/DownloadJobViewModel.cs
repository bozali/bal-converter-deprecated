using Bal.Converter.Events;
using Bal.Converter.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bal.Converter.Modules.Downloads.ViewModels;

public partial class DownloadJobViewModel : ObservableObject
{
    private readonly IDownloadsRegistryService downloadsRegistry;
    private readonly DownloadJob job;

    [ObservableProperty] private int id;
    [ObservableProperty] private string url;
    [ObservableProperty] private string title;
    [ObservableProperty] private float progress;
    [ObservableProperty] private DownloadState state;
    [ObservableProperty] private string stateIcon;
    [ObservableProperty] private string progressText;

    public DownloadJobViewModel(IDownloadsRegistryService downloadsRegistry, DownloadJob job)
    {
        this.downloadsRegistry = downloadsRegistry;
        this.job = job;

        this.Id = this.job.Id;
        this.Url = this.job.Url;
        this.State = this.job.State;
        this.Title = this.job.Tags == null ? this.job.Url : this.job.Tags.Title;

        this.job.StateChanged += this.OnDownloadStateChanged;
        this.job.ProgressChanged += this.OnProgressChanged;
    }

    [RelayCommand]
    public void Cancel()
    {
        this.downloadsRegistry.UpdateState(this.job, DownloadState.Cancelled);
    }

    private void OnDownloadStateChanged(object sender, DownloadStateChangedEventArgs e)
    {
        this.State = e.State;
        this.Title = this.job.Tags == null ? this.job.Url : this.job.Tags.Title;
    }

    private void OnProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        this.Progress = e.Progress;
        this.ProgressText = e.Text;
    }

    partial void OnStateChanged(DownloadState value)
    {
        switch (value)
        {
            case DownloadState.Fetching:
                this.StateIcon = "\xe895";
                break;

            case DownloadState.Downloading:
                this.StateIcon = "\xe896";
                break;

            case DownloadState.Done:
                this.StateIcon = "\xe73e";
                break;

            case DownloadState.Cancelled:
                this.StateIcon = "\xe71a";
                break;

            case DownloadState.Pending:
            default:
                this.StateIcon = "\xe81c";
                break;
        }


    }
}