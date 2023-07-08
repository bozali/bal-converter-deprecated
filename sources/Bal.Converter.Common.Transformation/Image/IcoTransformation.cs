using Bal.Converter.Common.Transformation.Attributes;
using Bal.Converter.Common.Transformation.Constants;

namespace Bal.Converter.Common.Transformation.Image;


[Extension(FileExtensions.Image.Ico)]
[Target(typeof(IcoTransformation))]
[Target(typeof(PngTransformation))]
public class IcoTransformation : DefaultImageTransformation<IcoTransformation>
{
}