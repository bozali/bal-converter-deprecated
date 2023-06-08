using System.Collections;
using System.Diagnostics;

using Bal.Converter.Common;
using Bal.Converter.FFmpeg.Filters.Audio;
using Bal.Converter.FFmpeg.Filters.Video;

namespace Bal.Converter.FFmpeg;

public class FFmpeg : IFFmpeg
{
    private readonly string ffmpegPath;

    public FFmpeg(string ffmpegPath)
    {
        this.ffmpegPath = ffmpegPath;
    }

    public async Task Convert(string path, string destination, ConversionOptions options)
    {
        var arguments = new List<string>
        {
            $@"-i ""{path}""",
        };

        if (options.StartPosition != null)
        {
            arguments.Add($@"-ss ""{options.StartPosition?.ToString($"hh\\:mm\\:ss")}""");
        }

        if (options.EndPosition != null)
        {
            arguments.Add($@"-t ""{options.EndPosition.Value.ToString($"hh\\:mm\\:ss")}""");
        }

        if (options.Filters != null && options.Filters.Any())
        {
            arguments.Add(this.BuildFilterArguments(options.Filters));
        }

        arguments.Add($@"""{destination}""");

        try
        {
            using var process = new ProcessWrapper(this.ffmpegPath);
            process.ErrorDataReceived += this.OnDataReceived;
            process.OutputDataReceived += this.OnDataReceived;

            await process.Execute(string.Join(' ', arguments));

            process.ErrorDataReceived -= this.OnDataReceived;
            process.OutputDataReceived -= this.OnDataReceived;
        }
        catch (Exception e)
        {
            // TODO Error logging
        }
    }
    
    private void OnDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            Debug.WriteLine(e.Data);
        }
    }

    private string BuildFilterArguments(IEnumerable filters)
    {
        object[] enumerable = filters as object[] ?? filters.Cast<object>().ToArray();

        var videoFilters = enumerable.OfType<IVideoFilter>().ToArray();
        var audioFilters = enumerable.OfType<IAudioFilter>().ToArray();

        string videoFiltersArgument = string.Empty;
        string audioFiltersArgument = string.Empty;

        if (videoFilters.Any())
        {
            videoFiltersArgument += $"-vf {string.Join(' ', videoFilters.Select(f => $@"""{f.GetArgument()}"""))}";
        }

        if (audioFilters.Any())
        {
            audioFiltersArgument += $"-af {string.Join(' ', audioFilters.Select(f => $@"""{f.GetArgument()}"""))}";
        }

        return string.Join(' ', videoFiltersArgument, audioFiltersArgument);
    }
}