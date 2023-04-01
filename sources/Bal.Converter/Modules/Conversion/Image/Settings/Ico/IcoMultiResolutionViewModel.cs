using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.Conversion.Image.Settings.Ico;

public partial class IcoMultiResolutionViewModel : ObservableObject
{
    [ObservableProperty] private bool useMultiResolution;
    [ObservableProperty] private string resolutions;

    public IEnumerable<int> NumericResolutions
    {
        get
        {
            string[] items = this.resolutions.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return items.Where(x => int.TryParse(x, out int i)).Select(int.Parse);
        }
    }
}