using Bal.Converter.FFmpeg.Filters.Audio;

namespace Bal.Converter.Common.Conversion.Audio;

public class AudioConversionOptions
{
    public IAudioFilter[] AudioFilters { get; set; }
}