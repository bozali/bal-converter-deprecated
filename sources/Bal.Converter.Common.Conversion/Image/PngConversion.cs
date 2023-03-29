using Bal.Converter.Common.Conversion.Attributes;
using Bal.Converter.Common.Conversion.Constants;
using ImageMagick;

namespace Bal.Converter.Common.Conversion.Image;

[Extension(FileExtensions.Image.Png)]
[Target(typeof(PngConversion))]
[Target(typeof(GifConversion))]
[Target(typeof(BmpConversion))]
[Target(typeof(JpegConversion))]
public class PngConversion : ConversionBase<PngConversion>, IImageConversion
{
    public PngConversion()
    {
    }

    public override ConversionTopology Topology
    {
        get => ConversionTopology.Image;
    }

    public ImageConversionOptions ImageConversionOptions { get; set; }

    public override async Task Convert(string source, string destination)
    {
        string target = Path.GetExtension(destination);

        var magic = new MagickImage(source);
        await magic.WriteAsync(destination, Enum.Parse<MagickFormat>(target, true));
    }
}