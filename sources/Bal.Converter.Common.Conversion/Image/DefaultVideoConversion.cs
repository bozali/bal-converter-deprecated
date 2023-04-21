using Bal.Converter.Common.Conversion.Audio;
using Bal.Converter.Common.Conversion.Video;
using Bal.Converter.FFmpeg;
using Bal.Converter.FFmpeg.Filters;

namespace Bal.Converter.Common.Conversion.Image;

public class DefaultVideoConversion<T> : ConversionBase<T>, IVideoConversion, IAudioConversion where T : IConversion
{
    private readonly IFFmpeg ffmpeg;

    public DefaultVideoConversion(IFFmpeg ffmpeg)
    {
        this.ffmpeg = ffmpeg;
    }

    public override ConversionTopology Topology
    {
        get => ConversionTopology.Audio | ConversionTopology.Video;
    }

    public override async Task Convert(string source, string destination)
    {
        await this.ffmpeg.Convert(source, destination, new ConversionOptions
        {
            Filters = this.VideoConversionOptions.VideoFilters
                .Cast<IFilter>()
                .Concat(this.AudioConversionOptions.AudioFilters).ToArray()
        });
    }

    public VideoConversionOptions VideoConversionOptions { get; set; }

    public AudioConversionOptions AudioConversionOptions { get; set; }
}