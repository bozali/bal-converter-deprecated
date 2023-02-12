using Bal.Converter.Common.Enums;
using Bal.Converter.Common.Media;
using Bal.Converter.Events;
using Bal.Converter.YouTubeDl.Quality;

namespace Bal.Converter.Modules.Downloads ;

[Serializable]
public class DownloadJob
{
    private DownloadState state;

    public event DownloadStateChangedEventHandler? StateChanged;

    public DownloadJob(string url)
    {
        this.AutomaticQualityOption = QualityOption.BestQuality;
        this.State = DownloadState.Pending;
        this.Id = Guid.NewGuid();
        this.Url = url;
    }

    public Guid Id { get; set; }

    public string Url { get; set; }

    public MediaFileExtension TargetFormat { get; set; }

    public DownloadState State
    {
        get => this.state;
        set
        {
            this.state = value;
            this.OnStateChanged(this.state);
        }
    }

    public QualityOption AutomaticQualityOption { get; set; }

    public MediaTags? Tags { get; set; }

    protected virtual void OnStateChanged(DownloadState state)
    {
        this.StateChanged?.Invoke(this, new DownloadStateChangedEventArgs(state));
    }
}