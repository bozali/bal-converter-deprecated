using Bal.Converter.Common.Enums;
using Bal.Converter.YouTubeDl.Quality;

namespace Bal.Converter.Modules.Downloads ;

public class DownloadRegistrationOptions
{
    public MediaFileExtension Extension { get; set; }

    public QualityOption QualityOption { get; set; }
}