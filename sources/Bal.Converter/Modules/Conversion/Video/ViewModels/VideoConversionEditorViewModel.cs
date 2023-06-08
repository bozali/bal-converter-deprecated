#pragma warning disable CS8618
#pragma warning disable CS8601
#pragma warning disable CS8604
#pragma warning disable CS8602

using System.Collections.ObjectModel;

using Windows.Storage.Pickers;

using AutoMapper;

using Bal.Converter.Common.Conversion;
using Bal.Converter.Common.Conversion.Audio;
using Bal.Converter.Common.Conversion.Video;
using Bal.Converter.Common.Extensions;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.FFmpeg;
using Bal.Converter.FFmpeg.Filters.Audio;
using Bal.Converter.FFmpeg.Filters.Video;
using Bal.Converter.Modules.Conversion.Filters.Volume;
using Bal.Converter.Modules.Conversion.ViewModels;
using Bal.Converter.Modules.MediaDownloader.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml.Controls;
using Bal.Converter.Services;

namespace Bal.Converter.Modules.Conversion.Video.ViewModels;

public partial class VideoConversionEditorViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService navigationService;
    private readonly IFFmpeg ffmpeg;
    private readonly IMapper mapper;

    [ObservableProperty] private VideoConversionOptionsViewModel videoConversionOptions;
    [ObservableProperty] private VideoMetadataViewModel metadata;
    [ObservableProperty] private string? sourcePath;
    [ObservableProperty] private ObservableCollection<string> availableFilters;
    [ObservableProperty] private ObservableCollection<Page> filterPages;

    private IConversion conversion;

    public VideoConversionEditorViewModel(INavigationService navigationService, IFFmpeg ffmpeg, IMapper mapper)
    {
        this.navigationService = navigationService;
        this.ffmpeg = ffmpeg;
        this.mapper = mapper;

        this.VideoConversionOptions = new VideoConversionOptionsViewModel();
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

        this.conversion = input.Get<IConversion>("Conversion");
        this.SourcePath = input.Get<string>("SourcePath");

        this.SetMediaPlayerSource(this.SourcePath);

        if (this.conversion.Topology.HasFlag(ConversionTopology.Audio))
        {
            this.AvailableFilters.AddRange(new[] { FilterNameConstants.Audio.Volume });
        }

        if (this.conversion.Topology.HasFlag(ConversionTopology.Video))
        {
            this.AvailableFilters.AddRange(new[] { FilterNameConstants.Video.Rotation, FilterNameConstants.Video.Fps });
        }
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private void AddFilter(string filter)
    {
        switch (filter.ToLowerInvariant())
        {
            case "volume":
                this.FilterPages.Add(new VolumeFilterPage());
                break;
        }
    }

    [RelayCommand]
    private async Task Convert()
    {
        if (this.conversion.Topology.HasFlag(ConversionTopology.Video))
        {
            ((IVideoConversion)this.conversion).VideoConversionOptions = new VideoConversionOptions
            {
                VideoFilters = this.GetVideoFilters().ToArray()
            };
        }

        if (this.conversion.Topology.HasFlag(ConversionTopology.Audio))
        {
            ((IAudioConversion)this.conversion).AudioConversionOptions = new AudioConversionOptions
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

    private IEnumerable<IVideoFilter> GetVideoFilters()
    {
        foreach (var filter in this.FilterPages)
        {
            yield break;
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