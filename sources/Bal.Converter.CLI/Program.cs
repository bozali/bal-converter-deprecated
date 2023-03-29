using Bal.Converter.CLI.Verbs;

using CommandLine;

namespace Bal.Converter.CLI;

public class Program
{
    public static async Task Main(string[] args)
    {
        await Parser.Default.ParseArguments<ConvertVerb, DownloadVerb>(args)
            .MapResult(
                (ConvertVerb verb) => Task.FromResult(0),
                (DownloadVerb verb) => Task.FromResult(0),
                (IEnumerable<Error> errors) => Task.FromResult(0));
    }
}