namespace Bal.Converter.Events ;


public sealed class DownloadProgressChangedEventArgs : EventArgs
{
    public DownloadProgressChangedEventArgs(float progress, string text)
    {
        this.Progress = progress;
        this.Text = text;
    }

    public float Progress { get; set; }

    public string Text { get; set; }
}

public delegate void DownloadProgressChangedEventHandler(object sender, DownloadProgressChangedEventArgs e);
