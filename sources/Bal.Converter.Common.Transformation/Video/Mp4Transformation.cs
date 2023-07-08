
using Bal.Converter.Common.Transformation.Attributes;
using Bal.Converter.Common.Transformation.Audio;
using Bal.Converter.Common.Transformation.Constants;
using Bal.Converter.FFmpeg;

namespace Bal.Converter.Common.Transformation.Video;

[Extension(FileExtensions.Video.Mp4)]
[Target(typeof(Mp4Transformation))]
[Target(typeof(Mp3Transformation))]
[Target(typeof(AviTransformation))]
[Target(typeof(WavTransformation))]
public class Mp4Transformation : DefaultVideoTransformation<Mp4Transformation>
{
    public Mp4Transformation(IFFmpeg ffmpeg) : base(ffmpeg)
    {
    }
}