using Bal.Converter.FFmpeg.Filters.Video;

namespace Bal.Converter.Common.Conversion.Video;

public class VideoConversionOptions
{
    public IVideoFilter[] VideoFilters { get; set; }

    /// <summary>
    /// The lower the number the better the quality.
    /// </summary>
    public int Quality { get; set; }
}