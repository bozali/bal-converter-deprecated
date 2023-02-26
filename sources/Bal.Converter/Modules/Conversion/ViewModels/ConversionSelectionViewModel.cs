using System.Collections.ObjectModel;

using Bal.Converter.Common.Conversion;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.Conversion.ViewModels;

public partial class ConversionSelectionViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<string> supportedFormats;

    private readonly IConversionProvider conversionProvider;

    private string path;

    public ConversionSelectionViewModel(IConversionProvider conversionProvider)
    {
        this.conversionProvider = conversionProvider;
    }

    public void HandleDrop(string path)
    {
        this.path = path;
        this.SupportedFormats = new ObservableCollection<string>(this.conversionProvider.GetSupportedFormats(path));
    }
}