namespace Bal.Converter.FFmpeg.Filters.Video;

public class UnsharpFilter : IVideoFilter
{
    public int LumaHorizontalSize { get; set; }
    
    public int LumaVerticalSize { get; set; }

    public float LumaStrength { get; set; }

    public string GetArgument()
    {
        return $"unsharp=lx={this.LumaHorizontalSize}:ly={this.LumaVerticalSize}:la={this.LumaStrength:F}";
    }
}