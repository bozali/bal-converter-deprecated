using Newtonsoft.Json;

namespace Bal.Converter.UpdateManager.SlimClients;

[JsonObject]
[Serializable]
public class GithubAsset
{
    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("browser_download_url")]
    public string? BrowserDownloadUrl { get; set; }
}