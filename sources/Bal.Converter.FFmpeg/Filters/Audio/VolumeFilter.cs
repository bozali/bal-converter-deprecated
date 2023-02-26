namespace Bal.Converter.FFmpeg.Filters.Audio;

public class VolumeFilter : IAudioFilter
{
    public float Multiplier { get; set; }

    public int Decibel { get; set; }

    public bool UseDecibel { get; set; }

    public string GetArgument()
    {
        return !this.UseDecibel ? $"volume={this.Multiplier}" : $"volume={this.Decibel}";
    }
}