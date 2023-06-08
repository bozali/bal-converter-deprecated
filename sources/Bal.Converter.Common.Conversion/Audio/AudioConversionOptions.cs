using Bal.Converter.FFmpeg.Filters.Audio;

namespace Bal.Converter.Common.Conversion.Audio;

public class AudioConversionOptions
{
    public AudioConversionOptions()
    {
        this.AudioFilters = Enumerable.Empty<IAudioFilter>().ToArray();
    }

    public IAudioFilter[] AudioFilters { get; set; }

    public TimeSpan? StartPosition { get; set; }

    public TimeSpan? EndPosition { get; set; }
}