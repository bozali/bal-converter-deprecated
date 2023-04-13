using Bal.Converter.CLI.Executors;
using Bal.Converter.CLI.Verbs;
using Bal.Converter.Common.Conversion;
using Bal.Converter.Common.Conversion.Extensions;
using Bal.Converter.FFmpeg;
using CommandLine;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bal.Converter.CLI;

public class Program
{
    public static IHost Host { get; set; }

    public static async Task Main(string[] args)
    {
        Host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureServices(ConfigureServices)
            .Build();

        await Parser.Default.ParseArguments<ConvertVerb, DownloadVerb>(args)
            .MapResult(
                async (ConvertVerb verb) => await Host.Services.GetService<ConvertExecutor>()!.Execute(verb),
                (DownloadVerb verb) => Task.FromResult(0),
                (IEnumerable<Error> errors) => Task.FromResult(0));
    }

    public static void ConfigureServices(HostBuilderContext context, IServiceCollection collection)
    {
        collection.ConfigureConversions();

        collection
            .AddSingleton<IConversionProvider, ConversionProvider>()
            .AddSingleton<IFFmpeg, FFmpeg.FFmpeg>(provider => new FFmpeg.FFmpeg(@"Tools\ffmpeg.exe"))
            .AddTransient<ConvertExecutor>();
    }
}