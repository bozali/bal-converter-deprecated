using Newtonsoft.Json;

namespace Bal.Converter.UpdateManager.SlimClients;

public class SlimGithubClient : IDisposable
{
    private readonly HttpClient http;

    public SlimGithubClient(HttpClient http)
    {
        this.http = http;
    }

    public async Task Download(string url, string path)
    {
        using var response = await this.http.GetAsync(url);

        await using var fs = new FileStream(path, FileMode.CreateNew);

        await response.Content.CopyToAsync(fs);
    }

    public async Task<LatestTagResponse> GetLatestTag()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/yt-dlp/yt-dlp/releases/latest");

        request.Headers.Add("User-Agent", "Bal-Converter");

        using var response = await this.http.SendAsync(request);
        using var reader = new StreamReader(await response.Content.ReadAsStreamAsync());

        string content = await reader.ReadToEndAsync();

        return JsonConvert.DeserializeObject<LatestTagResponse>(content)!;
    }

    public void Dispose()
    {
        this.http.Dispose();
    }
}