using Newtonsoft.Json;

namespace Bal.Converter.YouTubeDl;

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public class Format
{
    [JsonProperty("format_id")]
    public string FormatId { get; set; }

    [JsonProperty("format_note")]
    public string FormatNote { get; set; }

    [JsonProperty("fps")]
    public float? Fps { get; set; }

    [JsonProperty("width")]
    public int? Width { get; set; }

    [JsonProperty("height")]
    public int? Height { get; set; }

    [JsonProperty("resolution")]
    public string Resolution { get; set; }

    [JsonProperty("ext")]
    public string Extension { get; set; }

    // [JsonProperty("filesize")]
    // public string FileSize { get; set; }

    [JsonProperty("vcodec")]
    public string VCodec { get; set; }

    [JsonProperty("acodec")]
    public string ACodec { get; set; }

    [JsonIgnore]
    public bool IsAudioOnly => string.Equals(this.ACodec, "video only", StringComparison.InvariantCultureIgnoreCase);

    [JsonIgnore]
    public bool IsVideoOnly => string.Equals(this.VCodec, "audio only", StringComparison.InvariantCultureIgnoreCase);

    [JsonIgnore]
    public bool IsVideoAndAudio => !this.IsAudioOnly && !this.IsVideoOnly;

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