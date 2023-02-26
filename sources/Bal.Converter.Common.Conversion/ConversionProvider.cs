using System.Reflection;

namespace Bal.Converter.Common.Conversion;

public class ConversionProvider : IConversionProvider
{
    public string[] GetSupportedFormats(string path)
    {
        string? extension = Path.GetExtension(path)?.Replace(".", string.Empty);

        if (string.IsNullOrEmpty(extension))
        {
            throw new ArgumentException($"Provided path {path} is not a file.");
        }

        var conversionTypes = Assembly
            .GetAssembly(typeof(IConversion))?.GetTypes()
            .Where(x => !x.IsAbstract && x.GetInterfaces().Contains(typeof(IConversion)));

        if (conversionTypes == null)
        {
            return Enumerable.Empty<string>().ToArray();
        }

        var found = conversionTypes.FirstOrDefault(x => string.Equals(ConversionAttributeExtractor.ExtractExtension(x), extension));

        if (found == null)
        {
            return Enumerable.Empty<string>().ToArray();
        }

        return ConversionAttributeExtractor.ExtractSupportedTypes(found).Select(x => ConversionAttributeExtractor.ExtractExtension(x).ToLowerInvariant()).ToArray();
    }
}