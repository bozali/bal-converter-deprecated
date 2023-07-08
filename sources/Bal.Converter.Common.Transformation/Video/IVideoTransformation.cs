namespace Bal.Converter.Common.Transformation.Video;

public interface IVideoTransformation : IFileTransformation
{
    VideoTransformationOptions VideoTransformationOptions { get; set; }
}