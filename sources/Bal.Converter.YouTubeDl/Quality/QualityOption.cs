namespace Bal.Converter.YouTubeDl.Quality ;

public class QualityOption
{
    public static readonly QualityOption BestQuality = new(AutomaticQualityOption.Best, AutomaticQualityOption.Best);
    public static readonly QualityOption WorstQuality = new(AutomaticQualityOption.Worst, AutomaticQualityOption.Worst);

    public QualityOption(AutomaticQualityOption audioQuality, AutomaticQualityOption videoQuality)
    {
        this.AudioQuality = audioQuality;
        this.VideoQuality = videoQuality;
    }

    public QualityOption()
    {
    }

    public AutomaticQualityOption AudioQuality { get; set; }

    public AutomaticQualityOption VideoQuality { get; set; }
}