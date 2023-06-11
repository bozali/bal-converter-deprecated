using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.Conversion.Filters.Unsharp;

public partial class UnsharpFilterViewModel : FilterBaseViewModel
{
    [ObservableProperty] private int lumaHorizontalSize;
    [ObservableProperty] private int lumaVerticalSize;
    [ObservableProperty] private float lumaStrength;

    public UnsharpFilterViewModel() : base("Unsharp")
    {
        this.LumaHorizontalSize = 5;
        this.LumaVerticalSize = 5;
        this.LumaStrength = 0.0f;
    }
}