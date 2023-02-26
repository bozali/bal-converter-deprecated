using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.Conversion.ViewModels;

public partial class VideoMetadataViewModel : ObservableObject
{
    [ObservableProperty] private int maximumLength;
}