namespace Bal.Converter.Common.Transformation;

public interface ITransformationProvider
{
    string[] GetSupportedFormats(string path);

    IFileTransformation Provide(string target);
}