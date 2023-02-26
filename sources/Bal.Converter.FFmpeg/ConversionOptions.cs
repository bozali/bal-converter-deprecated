using Bal.Converter.FFmpeg.Filters;

namespace Bal.Converter.FFmpeg ;

public class ConversionOptions
{
    public TimeSpan? StartPosition { get; set; }

    public TimeSpan? EndPosition { get; set; }

    public IFilter[] Filters { get; set; }
}