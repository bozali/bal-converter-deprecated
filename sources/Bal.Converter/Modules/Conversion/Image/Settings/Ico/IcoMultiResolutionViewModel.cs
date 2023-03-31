using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.Conversion.Image.Settings.Ico;

public partial class IcoMultiResolutionViewModel : ObservableObject
{
    [ObservableProperty] private bool useMultiResolution;
    [ObservableProperty] private ObservableCollection<int> resolutions;
}