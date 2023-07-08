#pragma warning disable CS8604
#pragma warning disable CS8601
#pragma warning disable CS8618
#pragma warning disable CS8600

using System.Collections.ObjectModel;

using Windows.Storage.Pickers;

using Bal.Converter.Common.Extensions;
using Bal.Converter.Common.Transformation;
using Bal.Converter.Common.Transformation.Constants;
using Bal.Converter.Common.Transformation.Image;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.Domain.Picker;
using Bal.Converter.Modules.Conversion.Filters.Watermark;
using Bal.Converter.Modules.Conversion.Image.Settings.Ico;
using Bal.Converter.Modules.MediaDownloader.ViewModels;
using Bal.Converter.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ImageMagick;

using Microsoft.UI.Xaml.Controls;

using CollectionExtensions = Bal.Converter.Common.Extensions.CollectionExtensions;

namespace Bal.Converter.Modules.Conversion.Image;

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

    private IFileTransformation transformation;
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

        this.transformation = input.Get<IFileTransformation>("Conversion");
        this.SourcePath = input.Get<string>("SourcePath");
        string target = input.Get<string>("Target");

        this.IndividualSettings.AddRange(this.GetIndividualPages(target));
        this.HasIndividualSettings = this.IndividualSettings.Any<Page>();

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
    }

    [RelayCommand]
    private async Task Convert()
    {
        // TODO If any effects apply them to the options
        ((IImageTransformation)this.transformation).ImageTransformationOptions = new ImageTransformationOptions
        {
            UpdatedImageData = this.image.ToByteArray()
        };

        var options = new FileSavePickerOptions
        {
            SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            SuggestedFileName = Path.GetFileNameWithoutExtension((string?)this.SourcePath),
        };

        options.FileTypeChoices.Add($".{this.transformation.Extension}", new List<string> { $".{this.transformation.Extension}" });

        var file = await this.pickerService.OpenFileSave(options);

        if (file != null)
        {
            // TODO We need to think how we can handle individual settings
            // if (this.IndividualSettings.Any())
            // {
            //     foreach (var page in this.IndividualSettings)
            //     {
            //     }
            // }

            await this.transformation.Transform(this.SourcePath, file.Path);
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