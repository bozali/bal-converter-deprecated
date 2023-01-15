using Newtonsoft.Json;

namespace Bal.Converter.YouTubeDl;

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public class VideoFormat
{
    [JsonProperty("format_id")]
    public string FormatId { get; set; }

    [JsonProperty("fps")]
    public float? Fps { get; set; }

    [JsonProperty("width")]
    public int? Width { get; set; }

    [JsonProperty("height")]
    public int? Height { get; set; }

    /// <summary>
    /// Average audio bit rate in KBit/s
    /// </summary>
    [JsonProperty("abr")]
    public float AverageAudioBitRate { get; set; }

    /// <summary>
    /// Average video bit rate in KBit/s
    /// </summary>
    [JsonProperty("vbr")]
    public float AverageVideoBitRate { get; set; }
}