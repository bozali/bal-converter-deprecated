using System.Reflection;

using Bal.Converter.CLI.Verbs;
using Bal.Converter.Common.Transformation;
using Bal.Converter.Common.Transformation.Audio;

namespace Bal.Converter.CLI.Executors;

public class ConvertExecutor
{
    private readonly ITransformationProvider transformationProvider;

    public ConvertExecutor(ITransformationProvider transformationProvider)
    {
        this.transformationProvider = transformationProvider;
    }

    public async Task Execute(ConvertVerb verb)
    {
        Console.WriteLine(Assembly.GetExecutingAssembly().Location);
        Console.WriteLine(verb.Path);
        Console.WriteLine(verb.Destination);

        string[] supported = this.transformationProvider.GetSupportedFormats(verb.Path);

        string? targetExtension = Path.GetExtension(verb.Destination)?.Replace(".", string.Empty);

        if (string.IsNullOrEmpty(targetExtension) || !supported.Contains(targetExtension))
        {
            Console.WriteLine($"Target format {targetExtension} is not supported");
            return;
        }

        var conversion = this.transformationProvider.Provide(targetExtension);

        if (conversion.Topology.HasFlag(TransformationTopology.Audio))
        {
            ((IAudioTransformation)conversion).AudioTransformationOptions = new AudioTransformationOptions();
        }

        await conversion.Transform(verb.Path, verb.Destination);
    }
}