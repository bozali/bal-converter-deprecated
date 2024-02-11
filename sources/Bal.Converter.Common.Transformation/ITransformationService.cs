namespace Bal.Converter.Common.Transformation;

public interface ITransformationService
{
    string[] GetSupportedFormats(string path);

    IFileTransformation Provide(string target);
}