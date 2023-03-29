using Bal.Converter.Common.Conversion.Attributes;
using Bal.Converter.Common.Conversion.Constants;

namespace Bal.Converter.Common.Conversion.Image;

[Extension(FileExtensions.Image.Gif)]
[Target(typeof(PngConversion))]
[Target(typeof(GifConversion))]
[Target(typeof(BmpConversion))]
[Target(typeof(JpegConversion))]
public class GifConversion : DefaultImageConversion<GifConversion>
{
}