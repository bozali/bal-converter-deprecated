using Bal.Converter.Common.Enums;
using Bal.Converter.YouTubeDl.Quality;

namespace Bal.Converter.YouTubeDl;

public class DownloadOptions
{
    public DownloadOptions()
    {
        this.DownloadExtension = MediaFileExtension.MP4;
        this.DownloadBandwidth = 0;

        this.Quality = new QualityOption();
    }

    public int DownloadBandwidth { get; set; }

    public string Destination { get; set; }

    public MediaFileExtension DownloadExtension { get; set; }

    public QualityOption Quality { get; set; }
}