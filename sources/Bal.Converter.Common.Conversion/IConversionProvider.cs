namespace Bal.Converter.Common.Conversion;

public interface IConversionProvider
{
    string[] GetSupportedFormats(string path);

    IConversion Provide(string target);
}