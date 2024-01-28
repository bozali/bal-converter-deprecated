using Newtonsoft.Json;

namespace Bal.Converter.UpdateManager.SlimClients;

[JsonObject]
[Serializable]
public class LatestTagResponse
{
    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("tag_name")]
    public string? TagName { get; set; }

    [JsonProperty("assets")]
    public List<GithubAsset> Assets { get; set; }
}