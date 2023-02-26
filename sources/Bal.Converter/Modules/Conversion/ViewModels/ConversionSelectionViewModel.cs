using System.Collections.ObjectModel;

using Bal.Converter.Common.Conversion;
using Bal.Converter.Contracts.Services;
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
        // var conversion = this.conversionProvider.
        var parameters = new Dictionary<string, object>
        {
            { "SourcePath", this.path }
        };

        this.navigationService.NavigateTo(typeof(VideoConversionEditorViewModel).FullName!, parameters);
    }
}