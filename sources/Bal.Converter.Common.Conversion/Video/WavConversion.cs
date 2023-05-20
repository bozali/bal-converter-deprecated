using Bal.Converter.Common.Conversion.Attributes;
using Bal.Converter.Common.Conversion.Audio;
using Bal.Converter.Common.Conversion.Constants;
using Bal.Converter.FFmpeg;

namespace Bal.Converter.Common.Conversion.Video;

[Extension(FileExtensions.Video.Wav)]
[Target(typeof(Mp3Conversion))]
[Target(typeof(Mp4Conversion))]
[Target(typeof(WavConversion))]
[Target(typeof(AviConversion))]
public class WavConversion : DefaultVideoConversion<WavConversion>
{
    public WavConversion(IFFmpeg ffmpeg) : base(ffmpeg)
    {
    }
}