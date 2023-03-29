#pragma warning disable CS8604
#pragma warning disable CS8601

using Bal.Converter.Common.Conversion;
using Bal.Converter.Common.Extensions;
using Bal.Converter.Contracts.Services;
using Bal.Converter.Contracts.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageMagick;

namespace Bal.Converter.Modules.Conversion.Image.ViewModels;

public partial class ImageConversionEditorViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService navigationService;

    [ObservableProperty] private string? sourcePath;

    private IConversion conversion;
    private MagickImage image;

    public ImageConversionEditorViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    public Action<string> SetImageSource { get; set; }

    public Func<byte[], Task> UpdateImage { get; set; }

    public void OnNavigatedTo(object parameter)
    {
        var input = (IDictionary<string, object>)parameter;

        this.conversion = input.Get<IConversion>("Conversion");
        this.SourcePath = input.Get<string>("SourcePath");

        this.SetImageSource(this.SourcePath);

        this.image = new MagickImage(new FileInfo(this.SourcePath));
        this.UpdateImage(this.image.ToByteArray());
    }

    public void OnNavigatedFrom()
    {
        if (!this.image.IsDisposed)
        {
            this.image.Dispose();
        }
    }

    [RelayCommand]
    private void AddEffect(string effect)
    {
        using var watermark = new MagickImage(@"C:\Users\alibo\Downloads\character.png");
        watermark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 4);

        this.image.Composite(watermark, Gravity.Center, CompositeOperator.Over);

        this.UpdateImage(this.image.ToByteArray());
    }
}