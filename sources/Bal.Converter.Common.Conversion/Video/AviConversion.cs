using Bal.Converter.Common.Conversion.Attributes;
using Bal.Converter.Common.Conversion.Audio;
using Bal.Converter.Common.Conversion.Constants;
using Bal.Converter.Common.Conversion.Image;
using Bal.Converter.FFmpeg;

namespace Bal.Converter.Common.Conversion.Video;

[Extension(FileExtensions.Video.Avi)]
[Target(typeof(Mp3Conversion))]
[Target(typeof(Mp4Conversion))]
[Target(typeof(WavConversion))]
[Target(typeof(AviConversion))]
public class AviConversion : DefaultVideoConversion<AviConversion>
{
    public AviConversion(IFFmpeg ffmpeg) : base(ffmpeg)
    {
    }
}