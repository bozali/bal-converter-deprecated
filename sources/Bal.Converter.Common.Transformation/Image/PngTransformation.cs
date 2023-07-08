using Bal.Converter.Common.Transformation.Attributes;
using Bal.Converter.Common.Transformation.Constants;

namespace Bal.Converter.Common.Transformation.Image;

[Extension(FileExtensions.Image.Png)]
[Target(typeof(PngTransformation))]
[Target(typeof(GifTransformation))]
[Target(typeof(BmpTransformation))]
[Target(typeof(IcoTransformation))]
[Target(typeof(JpegTransformation))]
public class PngTransformation : DefaultImageTransformation<PngTransformation>
{
}