using Bal.Converter.Common.Conversion.Attributes;
using Bal.Converter.Common.Conversion.Constants;

namespace Bal.Converter.Common.Conversion.Audio;

[Extension(FileExtensions.Audio.Mp3)]
// [Target(typeof(WavConversion))]
[Target(typeof(Mp3Conversion))]
public class Mp3Conversion : ConversionBase<Mp3Conversion>, IAudioConversion
{
    public override ConversionTopology Topology
    {
        get => ConversionTopology.Audio;
    }

    public AudioConversionOptions AudioConversionOptions { get; set; }

    public override Task Convert(string source, string destination)
    {
        return Task.CompletedTask;
    }
}