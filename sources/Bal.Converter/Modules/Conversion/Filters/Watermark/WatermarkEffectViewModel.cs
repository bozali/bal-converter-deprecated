using Windows.Storage.Pickers;

using Bal.Converter.Domain.Picker;
using Bal.Converter.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageMagick;

namespace Bal.Converter.Modules.Conversion.Filters.Watermark;

public partial class WatermarkEffectViewModel : FilterBaseViewModel
{
    private readonly IDialogPickerService pickerService;

    [ObservableProperty] private int alphaChannelDivideValue;
    [ObservableProperty] private Gravity gravity;
    [ObservableProperty] private string imagePath;

    public WatermarkEffectViewModel(IDialogPickerService pickerService)
        : base("Watermark")
    {
        this.pickerService = pickerService;

        this.AlphaChannelDivideValue = 4;
        this.Gravity = Gravity.Center;
    }

    [RelayCommand]
    private async Task ChangeImagePath()
    {
        var options = new FileOpenPickerOptions
        {
            FileTypeFilter = new List<string> { "*" },
            SuggestedStartLocation = PickerLocationId.Desktop
        };

        var file = await this.pickerService.OpenFile(options);

        if (file != null)
        {
            this.ImagePath = file.Path;
        }
    }
}