using Bal.Converter.Common.Transformation.Audio;
using Bal.Converter.FFmpeg;
using Bal.Converter.FFmpeg.Filters;

namespace Bal.Converter.Common.Transformation.Video;

public class DefaultVideoTransformation<T> : FileTransformerBase<T>, IVideoTransformation, IAudioTransformation where T : IFileTransformation
{
    private readonly IFFmpeg ffmpeg;

    public DefaultVideoTransformation(IFFmpeg ffmpeg)
    {
        this.ffmpeg = ffmpeg;
    }

    public override TransformationTopology Topology
    {
        get => TransformationTopology.Audio | TransformationTopology.Video;
    }

    public override async Task Transform(string source, string destination)
    {
        await this.ffmpeg.Convert(source, destination, new ConversionOptions
        {
            Filters = this.VideoTransformationOptions.VideoFilters
                .Cast<IFilter>()
                .Concat(this.AudioTransformationOptions.AudioFilters).ToArray()
        });
    }

    public VideoTransformationOptions VideoTransformationOptions { get; set; }

    public AudioTransformationOptions AudioTransformationOptions { get; set; }
}