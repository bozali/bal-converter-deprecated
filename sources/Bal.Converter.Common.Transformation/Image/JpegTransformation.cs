using Bal.Converter.Common.Transformation.Attributes;
using Bal.Converter.Common.Transformation.Constants;

namespace Bal.Converter.Common.Transformation.Image;

[Extension(FileExtensions.Image.Jpeg)]
[Target(typeof(PngTransformation))]
[Target(typeof(GifTransformation))]
[Target(typeof(BmpTransformation))]
[Target(typeof(JpegTransformation))]
public class JpegTransformation : DefaultImageTransformation<JpegTransformation>
{
}