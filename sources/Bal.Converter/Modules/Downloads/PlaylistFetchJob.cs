using Bal.Converter.Common.Enums;
using Bal.Converter.YouTubeDl.Quality;

namespace Bal.Converter.Modules.Downloads;

public class PlaylistFetchJob
{
    public string Url { get; set; }

    public MediaFileExtension TargetFormat { get; set; }

    public QualityOption AutomaticQualityOption { get; set; }
}