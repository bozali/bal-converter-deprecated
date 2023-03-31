using Bal.Converter.Common.Conversion.Attributes;
using Bal.Converter.Common.Conversion.Constants;
using ImageMagick;

namespace Bal.Converter.Common.Conversion.Image;

[Extension(FileExtensions.Image.Png)]
[Target(typeof(PngConversion))]
[Target(typeof(GifConversion))]
[Target(typeof(BmpConversion))]
[Target(typeof(IcoConversion))]
[Target(typeof(JpegConversion))]
public class PngConversion : DefaultImageConversion<PngConversion>
{
}