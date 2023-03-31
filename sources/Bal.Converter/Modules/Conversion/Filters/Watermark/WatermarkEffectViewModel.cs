using CommunityToolkit.Mvvm.ComponentModel;

using ImageMagick;

namespace Bal.Converter.Modules.Conversion.Filters.Watermark;

public partial class WatermarkEffectViewModel : FilterBaseViewModel
{
    [ObservableProperty] private int alphaChannelDivideValue;
    [ObservableProperty] private Gravity gravity;

    public WatermarkEffectViewModel()
        : base("Watermark")
    {
        this.AlphaChannelDivideValue = 4;
        this.Gravity = Gravity.Center;
    }
}