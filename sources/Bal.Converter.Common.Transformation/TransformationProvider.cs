using System.Reflection;
using Bal.Converter.Common.Extensions;

namespace Bal.Converter.Common.Transformation;

public class TransformationProvider : ITransformationProvider
{
    private readonly IServiceProvider serviceProvider;

    public TransformationProvider(IServiceProvider serviceProvider)
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
            .GetAssembly(typeof(IFileTransformation))?.GetTypes()
            .Where(x => !x.IsAbstract && x.GetInterfaces().Contains(typeof(IFileTransformation)));

        if (conversionTypes == null)
        {
            return Enumerable.Empty<string>().ToArray();
        }

        var found = conversionTypes.FirstOrDefault(x => string.Equals(TransformationAttributeExtractor.ExtractExtension(x), extension));

        if (found == null)
        {
            return Enumerable.Empty<string>().ToArray();
        }

        return TransformationAttributeExtractor.ExtractSupportedTypes(found).Select(x => TransformationAttributeExtractor.ExtractExtension(x).ToTitleCase()).ToArray();
    }

    public IFileTransformation Provide(string target)
    {
        var conversionTypes = Assembly.GetAssembly(typeof(IFileTransformation))
                                      ?.GetTypes()
                                       .Where(t => !t.IsAbstract && t.GetInterfaces().Contains(typeof(IFileTransformation)))
                                       .ToArray();

        if (conversionTypes == null)
        {
            throw new Exception($"Could not find conversion for {target}.");
        }

        var found = conversionTypes.FirstOrDefault(t => string.Equals(TransformationAttributeExtractor.ExtractExtension(t), target, StringComparison.InvariantCultureIgnoreCase));

        if (found == null)
        {
            throw new Exception($"Could not find conversion for {target}.");
        }

        return this.serviceProvider.GetService(found) as IFileTransformation ?? throw new ApplicationException($"Could not find the conversion type for {target}");
    }
}