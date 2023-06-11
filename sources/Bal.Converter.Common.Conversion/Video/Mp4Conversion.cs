using Bal.Converter.Common.Conversion.Attributes;
using Bal.Converter.Common.Conversion.Audio;
using Bal.Converter.Common.Conversion.Constants;
using Bal.Converter.FFmpeg;

namespace Bal.Converter.Common.Conversion.Video;

[Extension(FileExtensions.Video.Mp4)]
[Target(typeof(Mp4Conversion))]
[Target(typeof(Mp3Conversion))]
[Target(typeof(AviConversion))]
[Target(typeof(WavConversion))]
public class Mp4Conversion : DefaultVideoConversion<Mp4Conversion>
{
    public Mp4Conversion(IFFmpeg ffmpeg) : base(ffmpeg)
    {
    }
}