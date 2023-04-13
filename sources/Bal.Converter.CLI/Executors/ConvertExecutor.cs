using System.Reflection;
using Bal.Converter.CLI.Verbs;
using Bal.Converter.Common.Conversion;
using Bal.Converter.Common.Conversion.Audio;

namespace Bal.Converter.CLI.Executors;

public class ConvertExecutor
{
    private readonly IConversionProvider conversionProvider;

    public ConvertExecutor(IConversionProvider conversionProvider)
    {
        this.conversionProvider = conversionProvider;
    }

    public async Task Execute(ConvertVerb verb)
    {
        Console.WriteLine(Assembly.GetExecutingAssembly().Location);
        Console.WriteLine(verb.Path);
        Console.WriteLine(verb.Destination);

        string[] supported = this.conversionProvider.GetSupportedFormats(verb.Path);

        string? targetExtension = Path.GetExtension(verb.Destination)?.Replace(".", string.Empty);

        if (string.IsNullOrEmpty(targetExtension) || !supported.Contains(targetExtension))
        {
            Console.WriteLine($"Target format {targetExtension} is not supported");
            return;
        }

        var conversion = this.conversionProvider.Provide(targetExtension);

        if (conversion.Topology.HasFlag(ConversionTopology.Audio))
        {
            ((IAudioConversion)conversion).AudioConversionOptions = new AudioConversionOptions();
        }

        await conversion.Convert(verb.Path, verb.Destination);
    }
}