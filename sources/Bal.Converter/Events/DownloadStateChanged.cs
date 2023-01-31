using Bal.Converter.Modules.Downloads;

namespace Bal.Converter.Events ;

public class DownloadStateChangedEventArgs : EventArgs
{
    public DownloadStateChangedEventArgs(DownloadState state)
    {
        this.State = state;
    }

    public virtual DownloadState State { get; set; }
}

public delegate void DownloadStateChangedEventHandler(object sender, DownloadStateChangedEventArgs e);
