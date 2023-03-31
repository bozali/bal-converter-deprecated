using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.Conversion.Image.Effects;

public partial class ImageEffectBaseViewModel : ObservableObject
{
    [ObservableProperty] private string displayName;

    protected ImageEffectBaseViewModel(string displayName)
    {
        this.DisplayName = displayName;
    }
}