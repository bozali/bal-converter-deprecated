using Bal.Converter.FFmpeg.Filters.Video;

namespace Bal.Converter.Common.Transformation.Video;

public class VideoTransformationOptions
{
    public IVideoFilter[] VideoFilters { get; set; }

    /// <summary>
    /// The lower the number the better the quality.
    /// </summary>
    public int Quality { get; set; }
}