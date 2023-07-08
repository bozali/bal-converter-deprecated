using Bal.Converter.Common.Transformation.Attributes;
using Bal.Converter.Common.Transformation.Constants;
using Bal.Converter.FFmpeg;

namespace Bal.Converter.Common.Transformation.Audio;

[Extension(FileExtensions.Audio.Mp3)]
// [Target(typeof(WavConversion))]
[Target(typeof(Mp3Transformation))]
public class Mp3Transformation : FileTransformerBase<Mp3Transformation>, IAudioTransformation
{
    private readonly IFFmpeg ffmpeg;

    public Mp3Transformation(IFFmpeg ffmpeg)
    {
        this.ffmpeg = ffmpeg;
    }

    public override TransformationTopology Topology
    {
        get => TransformationTopology.Audio;
    }

    public AudioTransformationOptions AudioTransformationOptions { get; set; }

    public override async Task Transform(string source, string destination)
    {
        await this.ffmpeg.Convert(source, destination, new ConversionOptions
        {
            // ReSharper disable once CoVariantArrayConversion
            Filters = this.AudioTransformationOptions.AudioFilters,
            StartPosition = this.AudioTransformationOptions.StartPosition,
            EndPosition = this.AudioTransformationOptions.EndPosition
        });
    }
}