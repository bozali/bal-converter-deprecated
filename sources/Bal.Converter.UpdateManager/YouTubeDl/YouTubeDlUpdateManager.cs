using System.Diagnostics;
using Bal.Converter.Common;
using Bal.Converter.UpdateManager.SlimClients;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Bal.Converter.UpdateManager.YouTubeDl;

public class YouTubeDlUpdateManager : IYouTubeDlUpdateManager
{
    private readonly ILogger<YouTubeDlUpdateManager> logger;
    private readonly SlimGithubClient client;
    private readonly string youtubeDlPath;

    public YouTubeDlUpdateManager(ILogger<YouTubeDlUpdateManager> logger, SlimGithubClient client, string youtubeDlPath)
    {
        this.logger = logger;
        this.client = client;
        this.youtubeDlPath = youtubeDlPath;
    }

    public async Task DownloadUpdate()
    {
        var response = await this.client.GetLatestTag();

        var asset = response.Assets.FirstOrDefault(x => string.Equals(x.Name, "yt-dlp.exe", StringComparison.OrdinalIgnoreCase));

        if (asset == null || string.IsNullOrEmpty(asset.BrowserDownloadUrl))
        {
            throw new Exception($"Could not find the asset for yt-dlp");
        }

        if (File.Exists(this.youtubeDlPath))
        {
            File.Delete(this.youtubeDlPath);
        }

        await this.client.Download(asset.BrowserDownloadUrl, this.youtubeDlPath);
    }

    public async Task<bool> HasNewVersion()
    {
        var response = await this.client.GetLatestTag();

        using var process = new ProcessWrapper(this.youtubeDlPath);

        string version = string.Empty;

        var handler = new DataReceivedEventHandler((s, e) =>
        {
            if (string.IsNullOrEmpty(e.Data))
            {
                return;
            }

            version = e.Data;
        });

        process.OutputDataReceived += handler;

        await process.Execute("--version").ConfigureAwait(true);

        process.OutputDataReceived -= handler;

        var localVersion = Version.Parse(version);
        var remoteVersion = Version.Parse(response.TagName!);

        return localVersion < remoteVersion;
    }

    private async Task<string> GetLatestVersion()
    {
        using var http = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/yt-dlp/yt-dlp/releases/latest");
        request.Headers.Add("User-Agent", "Bal-Converter");

        using var response = await http.SendAsync(request);
        using var reader = new StreamReader(await response.Content.ReadAsStreamAsync());
        
        string content = await reader.ReadToEndAsync();
        var json = JObject.Parse(content);

        string? tagName = json["tag_name"]?.Value<string>();

        if (string.IsNullOrEmpty(tagName))
        {
            throw new Exception($"Could not get the latest version for yt-dlp. https://api.github.com/repos/yt-dlp/yt-dlp/releases/latest");
        }

        return tagName;
    }
}