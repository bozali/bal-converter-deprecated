using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Bal.Converter.Common.Conversion;

public class ConversionProvider : IConversionProvider
{
    private readonly IServiceProvider serviceProvider;

    public ConversionProvider(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

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

    public IConversion Provide(string target)
    {
        var conversionTypes = Assembly.GetAssembly(typeof(IConversion))
                                      ?.GetTypes()
                                       .Where(t => !t.IsAbstract && t.GetInterfaces().Contains(typeof(IConversion)))
                                       .ToArray();

        if (conversionTypes == null)
        {
            throw new Exception($"Could not find conversion for {target}.");
        }

        var found = conversionTypes.FirstOrDefault(t => string.Equals(ConversionAttributeExtractor.ExtractExtension(t), target, StringComparison.InvariantCultureIgnoreCase));

        if (found == null)
        {
            throw new Exception($"Could not find conversion for {target}.");
        }

        return this.serviceProvider.GetService(found) as IConversion ?? throw new ApplicationException($"Could not find the conversion type for {target}");
    }
}