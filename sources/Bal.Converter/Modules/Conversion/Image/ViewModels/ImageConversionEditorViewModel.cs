#pragma warning disable CS8604
#pragma warning disable CS8601
#pragma warning disable CS8618

using Bal.Converter.Common.Conversion;
using Bal.Converter.Common.Conversion.Image;
using Bal.Converter.Common.Extensions;
using Bal.Converter.Contracts.Services;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.Modules.MediaDownloader.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageMagick;

using Microsoft.UI.Xaml.Controls;

using System.Collections.ObjectModel;

using Windows.Storage.Pickers;
using Bal.Converter.Modules.Conversion.Filters.Watermark;

namespace Bal.Converter.Modules.Conversion.Image.ViewModels;

public partial class ImageConversionEditorViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService navigationService;

    [ObservableProperty] private ObservableCollection<Page> effectPages;
    [ObservableProperty] private string? sourcePath;
    [ObservableProperty] private ObservableCollection<string> availableEffects;

    private IConversion conversion;
    private MagickImage image;

    public ImageConversionEditorViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;

        this.EffectPages = new ObservableCollection<Page>();
        this.AvailableEffects = new ObservableCollection<string>();
    }

    public Action<string> SetImageSource { get; set; }

    public Func<byte[], Task> UpdateImage { get; set; }

    public void OnNavigatedTo(object parameter)
    {
        this.EffectPages.Clear();

        var input = (IDictionary<string, object>)parameter;

        this.conversion = input.Get<IConversion>("Conversion");
        this.SourcePath = input.Get<string>("SourcePath");

        this.SetImageSource(this.SourcePath);

        this.image = new MagickImage(new FileInfo(this.SourcePath));
        this.UpdateImage(this.image.ToByteArray());

        this.AvailableEffects = new ObservableCollection<string>();

        this.AvailableEffects.AddRange(new[] { FilterNameConstants.Image.Watermark });
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
        switch (effect.ToLowerInvariant())
        {
            case "watermark":
                this.EffectPages.Add(new WatermarkEffectPage());
                break;
        }

        /*
        using var watermark = new MagickImage(@"C:\Users\alibo\Downloads\character.png");
        watermark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 4);
        
        this.image.Composite(watermark, Gravity.Center, CompositeOperator.Over);
        
        this.UpdateImage(this.image.ToByteArray());
        */
    }

    [RelayCommand]
    private async Task Save()
    {
        // TODO If any effects apply them to the options
        ((IImageConversion)this.conversion).ImageConversionOptions = new ImageConversionOptions
        {
            UpdatedImageData = this.image.ToByteArray()
        };  

        var picker = new FileSavePicker
        {
            SuggestedStartLocation = PickerLocationId.PicturesLibrary
        };

        picker.FileTypeChoices.Add($".{this.conversion.Extension}", new List<string> { $".{this.conversion.Extension}" });
        picker.SuggestedFileName = Path.GetFileNameWithoutExtension(this.SourcePath);

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        var file = await picker.PickSaveFileAsync();

        if (file != null)
        {
            await this.conversion.Convert(this.SourcePath, file.Path);
            this.navigationService.NavigateTo(typeof(MediaDownloaderViewModel).FullName!);
        }
    }
}