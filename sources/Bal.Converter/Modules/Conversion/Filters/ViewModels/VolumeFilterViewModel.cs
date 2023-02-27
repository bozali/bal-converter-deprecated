using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.Conversion.Filters.ViewModels;

public partial class VolumeFilterViewModel : FilterBaseViewModel
{
    [ObservableProperty] private float multiplier;
    [ObservableProperty] private int decibel;
    [ObservableProperty] private bool useDecibel;

    public VolumeFilterViewModel()
        : base("Volume")
    {
        this.Multiplier = 1.0f;
    }
}