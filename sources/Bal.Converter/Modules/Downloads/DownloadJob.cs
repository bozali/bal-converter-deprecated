// ReSharper disable ParameterHidesMember

using Bal.Converter.Common.Enums;
using Bal.Converter.Common.Media;
using Bal.Converter.Events;
using Bal.Converter.YouTubeDl.Quality;
using LiteDB;

namespace Bal.Converter.Modules.Downloads ;

[Serializable]
public class DownloadJob
{
    private DownloadState state;
    private string progressText;
    private float progress;

    public event DownloadStateChangedEventHandler? StateChanged;
    public event DownloadProgressChangedEventHandler? ProgressChanged;

    public DownloadJob(string url)
    {
        this.AutomaticQualityOption = QualityOption.BestQuality;
        this.State = DownloadState.Pending;
        this.Url = url;
    }

    [BsonId]
    public int Id { get; set; }

    [BsonField]
    public string Url { get; set; }

    [BsonField]
    public MediaFileExtension TargetFormat { get; set; }

    [BsonField]
    public string? ThumbnailPath { get; set; }

    [BsonField]
    public DownloadState State
    {
        get => this.state;
        set
        {
            this.state = value;
            this.ProgressText = this.state.ToString("G");

            this.OnStateChanged(this.state);
        }
    }

    [BsonIgnore]
    public string ProgressText
    {
        get => this.progressText;
        set
        {
            this.progressText = value;
            this.OnProgressChanged(this.Progress, this.ProgressText);
        }
    }

    [BsonIgnore]
    public float Progress
    {
        get => this.progress;
        set
        {
            this.progress = value;
            this.OnProgressChanged(this.Progress, this.ProgressText);
        }
    }

    [BsonField]
    public QualityOption AutomaticQualityOption { get; set; }

    [BsonField]
    public MediaTags? Tags { get; set; }

    protected virtual void OnStateChanged(DownloadState state)
    {
        this.StateChanged?.Invoke(this, new DownloadStateChangedEventArgs(state));
    }

    protected virtual void OnProgressChanged(float progress, string text)
    {
        this.ProgressChanged?.Invoke(this, new DownloadProgressChangedEventArgs(progress, text));
    }
}