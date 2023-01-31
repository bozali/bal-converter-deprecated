using Bal.Converter.Common.Enums;
using Bal.Converter.Common.Media;
using Bal.Converter.Events;

namespace Bal.Converter.Modules.Downloads ;

[Serializable]
public class DownloadJob
{
    public event DownloadStateChangedEventHandler StateChanged;

    public DownloadJob()
    {
        this.State = DownloadState.Pending;
    }

    public string Url { get; set; }

    public MediaFileExtension TargetFormat { get; set; }

    public DownloadState State { get; set; }

    public MediaTags Tags { get; set; }
}