#pragma warning disable CS8618
#pragma warning disable CS8601
#pragma warning disable CS8604
#pragma warning disable CS8602

using System.Collections.ObjectModel;

using Windows.Storage.Pickers;

using AutoMapper;

using Bal.Converter.Common.Extensions;
using Bal.Converter.Common.Transformation;
using Bal.Converter.Common.Transformation.Audio;
using Bal.Converter.Common.Transformation.Video;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.FFmpeg;
using Bal.Converter.FFmpeg.Filters.Audio;
using Bal.Converter.FFmpeg.Filters.Video;
using Bal.Converter.Modules.Conversion.Filters.Unsharp;
using Bal.Converter.Modules.Conversion.Filters.Volume;
using Bal.Converter.Modules.Conversion.ViewModels;
using Bal.Converter.Modules.MediaDownloader.ViewModels;
using Bal.Converter.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml.Controls;

using CollectionExtensions = Bal.Converter.Common.Extensions.CollectionExtensions;

namespace Bal.Converter.Modules.Conversion.Video;

public partial class VideoConversionEditorViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService navigationService;
    private readonly IFFmpeg ffmpeg;
    private readonly IMapper mapper;

    [ObservableProperty] private Video.VideoConversionOptionsViewModel videoConversionOptions;
    [ObservableProperty] private VideoMetadataViewModel metadata;
    [ObservableProperty] private string? sourcePath;
    [ObservableProperty] private ObservableCollection<string> availableFilters;
    [ObservableProperty] private ObservableCollection<Page> filterPages;

    private IFileTransformation transformation;

    public VideoConversionEditorViewModel(INavigationService navigationService, IFFmpeg ffmpeg, IMapper mapper)
    {
        this.navigationService = navigationService;
        this.ffmpeg = ffmpeg;
        this.mapper = mapper;

        this.VideoConversionOptions = new Video.VideoConversionOptionsViewModel();
        this.Metadata = new VideoMetadataViewModel();
        this.FilterPages = new ObservableCollection<Page>();
        this.AvailableFilters = new ObservableCollection<string>();
    }

    public Action<string> SetMediaPlayerSource { get; set; }

    public void OnNavigatedTo(object parameter)
    {
        this.AvailableFilters.Clear();
        this.FilterPages.Clear();

        var input = (IDictionary<string, object>)parameter;

        this.transformation = input.Get<IFileTransformation>("Conversion");
        this.SourcePath = input.Get<string>("SourcePath");

        this.SetMediaPlayerSource(this.SourcePath);

        if (this.transformation.Topology.HasFlag(TransformationTopology.Audio))
        {
            this.AvailableFilters.AddRange(new[] { FilterNameConstants.Audio.Volume });
        }

        if (this.transformation.Topology.HasFlag(TransformationTopology.Video))
        {
            this.AvailableFilters.AddRange(new[] { FilterNameConstants.Video.Rotation, FilterNameConstants.Video.Fps, FilterNameConstants.Video.Unsharp });
        }
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private void AddFilter(string filter)
    {
        if (this.FilterPages.Select<Page, string>(x => x.Name.ToLowerInvariant()).Contains(filter.ToLowerInvariant()))
        {
            return;
        }

        switch (filter.ToLowerInvariant())
        {
            case "volume":
                this.FilterPages.Add(new VolumeFilterPage());
                break;

            case "unsharp":
                this.FilterPages.Add(new UnsharpFilterPage());
                break;
        }
    }

    [RelayCommand]
    private async Task Convert()
    {
        if (this.transformation.Topology.HasFlag(TransformationTopology.Video))
        {
            ((IVideoTransformation)this.transformation).VideoTransformationOptions = new VideoTransformationOptions
            {
                VideoFilters = this.GetVideoFilters().ToArray()
            };
        }

        if (this.transformation.Topology.HasFlag(TransformationTopology.Audio))
        {
            ((IAudioTransformation)this.transformation).AudioTransformationOptions = new AudioTransformationOptions
            {
                AudioFilters = this.GetAudioFilters().ToArray(),
                StartPosition = this.VideoConversionOptions.MinVideoLengthTime,
                EndPosition = this.VideoConversionOptions.MaxVideoLengthTime
            };
        }

        var picker = new FileSavePicker
        {
            SuggestedStartLocation = PickerLocationId.Desktop,
        };

        picker.FileTypeChoices.Add($".{this.transformation.Extension}", new List<string> { $".{this.transformation.Extension}" });
        picker.SuggestedFileName = Path.GetFileNameWithoutExtension((string?)this.SourcePath);

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        var file = await picker.PickSaveFileAsync();

        if (file != null)
        {
            await this.transformation.Transform(this.SourcePath, file.Path);
            this.navigationService.NavigateTo(typeof(MediaDownloaderViewModel).FullName!);
        }
    }

    [RelayCommand]
    private void ClearFilter()
    {
        this.FilterPages.Clear();
    }

    private IEnumerable<IVideoFilter> GetVideoFilters()
    {
        foreach (var filter in this.FilterPages)
        {
            if (string.Equals(filter.Name, FilterNameConstants.Video.Unsharp))
            {
                yield return this.mapper.Map<UnsharpFilterViewModel, UnsharpFilter>(((UnsharpFilterPage)filter).ViewModel);
            }
        }
    }

    private IEnumerable<IAudioFilter> GetAudioFilters()
    {
        foreach (var filter in this.FilterPages)
        {
            if (string.Equals(filter.Name, FilterNameConstants.Audio.Volume))
            {
                yield return this.mapper.Map<VolumeFilterViewModel, VolumeFilter>(((VolumeFilterPage)filter).ViewModel);
            }
        }
    }
}