using CommandLine;

namespace Bal.Converter.CLI.Verbs;

[Verb("convert")]
public class ConvertVerb
{
    [Option('p', "path")]
    public string Path { get; set; }

    [Option('d', "destination")]
    public string Destination { get; set; }
}