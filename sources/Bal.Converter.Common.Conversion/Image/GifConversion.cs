using Bal.Converter.Common.Conversion.Attributes;
using Bal.Converter.Common.Conversion.Constants;

namespace Bal.Converter.Common.Conversion.Image;

[Extension(FileExtensions.Image.Gif)]
[Target(typeof(PngConversion))]
[Target(typeof(GifConversion))]
[Target(typeof(BmpConversion))]
[Target(typeof(JpegConversion))]
public class GifConversion : ConversionBase<GifConversion>, IImageConversion
{
    public override ConversionTopology Topology
    {
        get => ConversionTopology.Image;
    }

    public ImageConversionOptions ImageConversionOptions { get; set; }

    public override Task Convert(string source, string destination)
    {
        throw new NotImplementedException();
    }
}