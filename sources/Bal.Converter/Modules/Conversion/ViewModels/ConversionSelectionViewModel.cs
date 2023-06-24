using System.Collections.ObjectModel;

using Windows.Storage.Pickers;

using Bal.Converter.Common.Conversion;
using Bal.Converter.Domain.Picker;
using Bal.Converter.Modules.Conversion.Image.ViewModels;
using Bal.Converter.Modules.Conversion.Video.ViewModels;
using Bal.Converter.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

#pragma warning disable CS8618

namespace Bal.Converter.Modules.Conversion.ViewModels;

public partial class ConversionSelectionViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<string> supportedFormats;
    [ObservableProperty] private string? selectedFormat;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsFileSelected))]
    private string path;
    
    private readonly INavigationService navigationService;
    private readonly IDialogPickerService dialog;
    private readonly IConversionProvider conversionProvider;

    public ConversionSelectionViewModel(INavigationService navigationService, IDialogPickerService dialog, IConversionProvider conversionProvider)
    {
        this.navigationService = navigationService;
        this.dialog = dialog;
        this.conversionProvider = conversionProvider;
    }

    public bool IsFileSelected
    {
        // ReSharper disable once ValueParameterNotUsed
        set {}
        get => !string.IsNullOrEmpty(this.Path);
    }

    public void HandleDrop(string path)
    {
        this.Path = path;
        this.SupportedFormats = new ObservableCollection<string>(this.conversionProvider.GetSupportedFormats(path));
        this.SelectedFormat = this.SupportedFormats.FirstOrDefault();
    }

    [RelayCommand]
    private async Task OpenFile()
    {
        var result = await this.dialog.OpenFile(new FileOpenPickerOptions
        {
            SuggestedStartLocation = PickerLocationId.Desktop,
            FileTypeFilter = new List<string> { "*" }
        });

        if (result != null)
        {
            this.Path = result.Path;
        }
    }

    [RelayCommand]
    private void Continue()
    {
        if (string.IsNullOrEmpty(this.SelectedFormat))
        {
            return;
        }

        var conversion = this.conversionProvider.Provide(this.SelectedFormat);
        var parameters = new Dictionary<string, object>
        {
            { "Conversion", conversion },
            { "SourcePath", this.Path },
            { "Target", this.SelectedFormat }
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