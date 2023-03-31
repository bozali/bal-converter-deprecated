using Bal.Converter.Common.Conversion.Attributes;
using Bal.Converter.Common.Conversion.Constants;

namespace Bal.Converter.Common.Conversion.Image;


[Extension(FileExtensions.Image.Ico)]
[Target(typeof(IcoConversion))]
public class IcoConversion : DefaultImageConversion<IcoConversion>
{
}