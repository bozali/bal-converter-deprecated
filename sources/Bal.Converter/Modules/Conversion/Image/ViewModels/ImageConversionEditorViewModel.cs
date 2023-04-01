#pragma warning disable CS8604
#pragma warning disable CS8601
#pragma warning disable CS8618
#pragma warning disable CS8600

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
using Bal.Converter.Common.Conversion.Constants;
using Bal.Converter.Domain.Picker;
using Bal.Converter.Modules.Conversion.Filters.Watermark;
using Bal.Converter.Modules.Conversion.Image.Settings.Ico;
using Bal.Converter.Services;

namespace Bal.Converter.Modules.Conversion.Image.ViewModels;

public partial class ImageConversionEditorViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService navigationService;
    private readonly IDialogPickerService pickerService;

    /// <summary>
    /// Special settings for individual conversion e.g. converting to ico can have an multi resolution.
    /// </summary>
    [ObservableProperty] private ObservableCollection<Page> individualSettings;

    /// <summary>
    /// Modular pages for filters.
    /// </summary>
    [ObservableProperty] private ObservableCollection<Page> filterPages;

    [ObservableProperty] private bool hasIndividualSettings;
    [ObservableProperty] private string? sourcePath;
    [ObservableProperty] private ObservableCollection<string> availableEffects;

    private IConversion conversion;
    private MagickImage image;

    public ImageConversionEditorViewModel(INavigationService navigationService, IDialogPickerService pickerService)
    {
        this.navigationService = navigationService;
        this.pickerService = pickerService;

        this.IndividualSettings = new ObservableCollection<Page>();
        this.FilterPages = new ObservableCollection<Page>();
        this.AvailableEffects = new ObservableCollection<string>();
    }

    public Action<string> SetImageSource { get; set; }

    public Func<byte[], Task> UpdateImage { get; set; }

    public void OnNavigatedTo(object parameter)
    {
        this.IndividualSettings.Clear();
        this.FilterPages.Clear();
        
        var input = (IDictionary<string, object>)parameter;

        this.conversion = input.Get<IConversion>("Conversion");
        this.SourcePath = input.Get<string>("SourcePath");
        string target = input.Get<string>("Target");

        this.IndividualSettings.AddRange(this.GetIndividualPages(target));
        this.HasIndividualSettings = this.IndividualSettings.Any();

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
        switch (effect)
        {
            case FilterNameConstants.Image.Watermark:
                this.FilterPages.Add(new WatermarkEffectPage());
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
    private async Task Convert()
    {
        // TODO If any effects apply them to the options
        ((IImageConversion)this.conversion).ImageConversionOptions = new ImageConversionOptions
        {
            UpdatedImageData = this.image.ToByteArray()
        };

        var options = new FilePickerOptions
        {
            SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            SuggestedFileName = Path.GetFileNameWithoutExtension(this.SourcePath),
        };

        options.FileTypeChoices.Add($".{this.conversion.Extension}", new List<string> { $".{this.conversion.Extension}" });

        var file = await this.pickerService.OpenFileSave(options);

        if (file != null)
        {
            await this.conversion.Convert(this.SourcePath, file.Path);
            this.navigationService.NavigateTo(typeof(MediaDownloaderViewModel).FullName!);
        }
    }

    private IEnumerable<Page> GetIndividualPages(string target)
    {
        if (string.Equals(target, FileExtensions.Image.Ico))
        {
            yield return new IcoMultiResolutionPage();
        }
    }
}