using Bal.Converter.Common.Conversion.Attributes;
using Bal.Converter.Common.Conversion.Audio;
using Bal.Converter.Common.Conversion.Constants;
using Bal.Converter.FFmpeg;

namespace Bal.Converter.Common.Conversion.Video;

[Extension(FileExtensions.Video.Mkv)]
[Target(typeof(Mp3Conversion))]
[Target(typeof(Mp4Conversion))]
[Target(typeof(WavConversion))]
[Target(typeof(AviConversion))]
public class MkvConversion : DefaultVideoConversion<MkvConversion>
{
    public MkvConversion(IFFmpeg ffmpeg) : base(ffmpeg)
    {
    }
}