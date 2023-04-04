using Newtonsoft.Json;

namespace Bal.Converter.YouTubeDl;

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public class Playlist
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("playlist_count")]
    public int PlaylistCount { get; set; }

    [JsonProperty("webpage_url")]
    public string Url { get; set; }

    [JsonIgnore]
    public IList<Video> Videos { get; set; }
}