using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

using Bal.Converter.Common;

using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;
using System.Web;

using Bal.Converter.Common.Enums;
using System;
using Bal.Converter.Common.Extensions;
using System.IO;
using System.Reflection;

namespace Bal.Converter.YouTubeDl ;

public class YouTubeDl : IYouTubeDl
{
    private readonly string youtubeDlPath;
    private readonly string ffmpegPath;
    private readonly string tempPath;

    public YouTubeDl(string youtubeDlPath, string ffmpegPath, string tempPath)
    {
        this.youtubeDlPath = youtubeDlPath;
        this.ffmpegPath = ffmpegPath;
        this.tempPath = tempPath;
    }

    public async Task<Video> GetVideo(string url)
    {
        string pathPattern = Path.Combine(this.tempPath, "%(id)s.%(ext)s");

        var arguments = new List<string>
        {
            $@"--output ""{pathPattern}""",
            $@"""{url}""",
            "--write-info-json",
            "--skip-download"
        };

        var uri = new Uri(url);
        var queries = HttpUtility.ParseQueryString(uri.Query);

        if (queries.AllKeys.Any(k => string.Equals(k, "list", StringComparison.OrdinalIgnoreCase)))
        {
            arguments.Add("--no-playlist");
        }

        string infoFileName = string.Empty;

        using var process = new ProcessWrapper(this.youtubeDlPath);

        var handler = new DataReceivedEventHandler((s, e) =>
        {
            string extracted = ExtractFilePath(e.Data);

            if (!string.IsNullOrEmpty(extracted))
            {
                infoFileName = extracted;
            }
        });

        process.OutputDataReceived += handler;

        await process.Execute(string.Join(' ', arguments));

        process.OutputDataReceived -= handler;

        string infoPath = Path.Combine(this.tempPath, infoFileName);
        using var reader = new StreamReader(infoPath);

        try
        {
            return JsonConvert.DeserializeObject<Video>(await reader.ReadToEndAsync(),
                                                        new IsoDateTimeConverter { DateTimeFormat = "yyyyMMdd" });
        }
        finally
        {
            reader.Close();

            new FileInfo(infoPath).Delete();
        }
    }

    public async Task<Playlist> GetPlaylist(string url)
    {
        string pathPattern = Path.Combine(this.tempPath, "%(id)s.%(ext)s");

        var arguments = new List<string>
        {
            $@"--output ""{pathPattern}""",
            $@"""{url}""",
            "--write-info-json",
            "--skip-download"
        };

        using var process = new ProcessWrapper(this.youtubeDlPath);

        var infoFileNames = new List<string>();
        string playlistFileName = string.Empty;

        var handler = new DataReceivedEventHandler((s, e) =>
        {
            string extracted = ExtractPlaylistPath(e.Data);

            if (!string.IsNullOrEmpty(extracted))
            {
                playlistFileName = Path.GetFileName(extracted);
            }
            else
            {
                extracted = ExtractFilePath(e.Data);

                if (!string.IsNullOrEmpty(extracted))
                {
                    infoFileNames.Add(extracted);
                }
            }
        });


        process.OutputDataReceived += handler;

        await process.Execute(string.Join(' ', arguments));

        process.OutputDataReceived -= handler;

        string fullPlaylistPath = Path.Combine(this.tempPath, playlistFileName);
        Playlist playlist;

        try
        {

            using var reader = new StreamReader(fullPlaylistPath);
            playlist = JsonConvert.DeserializeObject<Playlist>(
                await reader.ReadToEndAsync(),
                new IsoDateTimeConverter { DateTimeFormat = "yyyyMMdd" });
        }
        finally
        {
            new FileInfo(fullPlaylistPath).Delete();
        }

        playlist.Videos = new List<Video>();

        foreach (string file in infoFileNames)
        {
            string path = Path.Combine(this.tempPath, file);

            try
            {
                {
                    using var reader = new StreamReader(path);
                    playlist.Videos.Add(JsonConvert.DeserializeObject<Video>(await reader.ReadToEndAsync(), new IsoDateTimeConverter { DateTimeFormat = "yyyyMMdd" }));
                }
            }
            finally
            {
                new FileInfo(path).Delete();
            }
        }


        return playlist;
    }

    public async Task Download(string url, DownloadOptions options, Action<float, string> progressReport = null, CancellationToken ct = default)
    {
        var arguments = new List<string>
        {
            $@"--output ""{options.Destination}""",
            $@"""{url}""",
            $@"--format best",
            $@"--no-playlist"
        };

        if (options.DownloadExtension.IsAudioOnly())
        {
            arguments.Add("--extract-audio");
            arguments.Add($@"--audio-format {options.DownloadExtension.ToString("G").ToLowerInvariant()}");
        }
        else if (options.DownloadExtension != MediaFileExtension.MP4)
        {
            arguments.Add($@"--recode-video {options.DownloadExtension.ToString("G").ToLowerInvariant()}");
        }

        if (options.DownloadExtension != MediaFileExtension.MP4)
        {
            string location =  !Path.IsPathRooted(this.ffmpegPath)
                ? Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, this.ffmpegPath)
                : this.ffmpegPath;

            arguments.Add($@"--ffmpeg-location ""{location}""");
        }

        if (options.DownloadBandwidth != 0)
        {
            arguments.Add($@"--http-chunk-size {options.DownloadBandwidth}");
        }

        var percentageReg = new Regex(@"([^\s]+)\%");
        var detailsReg = new Regex(@"(?<=\[download\])(.*)(?=ETA)");

        void OnOutputDataReceived(object s, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data) || !e.Data.Contains("[download]") || !e.Data.Contains("ETA"))
            {
                return;
            }

            string progressStr = percentageReg.Match(e.Data).Groups[1].ToString();
            float progress = Convert.ToSingle(progressStr, CultureInfo.InvariantCulture);
            string details = detailsReg.Match(e.Data).ToString().Trim();

            var doubleSpaceReg = new Regex("[ ]{2,}", RegexOptions.None);
            details = doubleSpaceReg.Replace(details, " ");

            progressReport?.Invoke(progress, details);
        }

        using var process = new ProcessWrapper(this.youtubeDlPath);

        process.OutputDataReceived += OnOutputDataReceived;
        process.ErrorDataReceived += OnOutputDataReceived;

        await process.Execute(string.Join(' ', arguments), ct);

        process.OutputDataReceived -= OnOutputDataReceived;
        process.ErrorDataReceived -= OnOutputDataReceived;
    }

    private static string ExtractPlaylistPath(string? data)
    {
        if (string.IsNullOrEmpty(data) || !data.Contains(".info.json"))
        {
            return string.Empty;
        }

        var pattern = new Regex(@".*playlist.*\.info\.json$", RegexOptions.IgnoreCase);
        var match = pattern.Match(data);

        return match.Value;
    }

    private static string ExtractFilePath(string? data)
    {

        if (string.IsNullOrEmpty(data) || !data.Contains(".info.json"))
        {
            return string.Empty;
        }

        var pattern = new Regex(@"[\w-]+S*\.info.json");
        var match = pattern.Match(data);

        return match.Value;
    }
}