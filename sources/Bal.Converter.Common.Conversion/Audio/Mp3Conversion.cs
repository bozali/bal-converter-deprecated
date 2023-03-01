using Bal.Converter.Common.Conversion.Attributes;
using Bal.Converter.Common.Conversion.Constants;
using Bal.Converter.FFmpeg;

namespace Bal.Converter.Common.Conversion.Audio;

[Extension(FileExtensions.Audio.Mp3)]
// [Target(typeof(WavConversion))]
[Target(typeof(Mp3Conversion))]
public class Mp3Conversion : ConversionBase<Mp3Conversion>, IAudioConversion
{
    private readonly IFFmpeg ffmpeg;

    public Mp3Conversion(IFFmpeg ffmpeg)
    {
        this.ffmpeg = ffmpeg;
    }

    public override ConversionTopology Topology
    {
        get => ConversionTopology.Audio;
    }

    public AudioConversionOptions AudioConversionOptions { get; set; }

    public override async Task Convert(string source, string destination)
    {
        await this.ffmpeg.Convert(source, destination, new ConversionOptions
        {
            // ReSharper disable once CoVariantArrayConversion
            Filters = this.AudioConversionOptions.AudioFilters
        });
    }
}