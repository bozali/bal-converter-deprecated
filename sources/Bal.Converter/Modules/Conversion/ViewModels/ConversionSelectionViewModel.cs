using System.Collections.ObjectModel;

using Bal.Converter.Common.Conversion;
using Bal.Converter.Contracts.Services;
using Bal.Converter.Modules.Conversion.Image.ViewModels;
using Bal.Converter.Modules.Conversion.Video.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bal.Converter.Modules.Conversion.ViewModels;

public partial class ConversionSelectionViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<string> supportedFormats;

    private readonly INavigationService navigationService;
    private readonly IConversionProvider conversionProvider;

    private string path;

    public ConversionSelectionViewModel(INavigationService navigationService, IConversionProvider conversionProvider)
    {
        this.navigationService = navigationService;
        this.conversionProvider = conversionProvider;
    }

    public void HandleDrop(string path)
    {
        this.path = path;
        this.SupportedFormats = new ObservableCollection<string>(this.conversionProvider.GetSupportedFormats(path));
    }

    [RelayCommand]
    private void Continue(string format)
    {
        var conversion = this.conversionProvider.Provide(format);
        var parameters = new Dictionary<string, object>
        {
            { "Conversion", conversion },
            { "SourcePath", this.path }
        };

        if (conversion.Topology.HasFlag(ConversionTopology.Video) || conversion.Topology.HasFlag(ConversionTopology.Audio))
        {
            this.navigationService.NavigateTo(typeof(VideoConversionEditorViewModel).FullName!, parameters);
        }
        else if (conversion.Topology.HasFlag(ConversionTopology.Image))
        {
            this.navigationService.NavigateTo(typeof(ImageConversionEditorViewModel).FullName!, parameters);
        }
    }
}