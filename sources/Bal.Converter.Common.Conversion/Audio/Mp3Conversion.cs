namespace Bal.Converter.Common.Conversion.Audio;

public class Mp3Conversion : ConversionBase<Mp3Conversion>, IAudioConversion
{
    public override ConversionTopology Topology
    {
        get => ConversionTopology.Audio;
    }

    public override Task Convert(string source, string destination)
    {
        return Task.CompletedTask;
    }
}