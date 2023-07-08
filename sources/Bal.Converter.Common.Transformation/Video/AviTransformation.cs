using Bal.Converter.Common.Transformation.Attributes;
using Bal.Converter.Common.Transformation.Audio;
using Bal.Converter.Common.Transformation.Constants;
using Bal.Converter.FFmpeg;

namespace Bal.Converter.Common.Transformation.Video;

[Extension(FileExtensions.Video.Avi)]
[Target(typeof(Mp3Transformation))]
[Target(typeof(Mp4Transformation))]
[Target(typeof(WavTransformation))]
[Target(typeof(AviTransformation))]
public class AviTransformation : DefaultVideoTransformation<AviTransformation>
{
    public AviTransformation(IFFmpeg ffmpeg) : base(ffmpeg)
    {
    }
}