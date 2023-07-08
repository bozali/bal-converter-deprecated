
namespace Bal.Converter.Common.Transformation.Image;

public interface IImageTransformation : IFileTransformation
{
    ImageTransformationOptions ImageTransformationOptions { get; set; }
}