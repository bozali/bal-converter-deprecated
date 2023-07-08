using Bal.Converter.Common.Transformation.Attributes;
using Bal.Converter.Common.Transformation.Constants;

namespace Bal.Converter.Common.Transformation.Image;

[Extension(FileExtensions.Image.Gif)]
[Target(typeof(PngTransformation))]
[Target(typeof(GifTransformation))]
[Target(typeof(BmpTransformation))]
[Target(typeof(JpegTransformation))]
public class GifTransformation : DefaultImageTransformation<GifTransformation>
{
}