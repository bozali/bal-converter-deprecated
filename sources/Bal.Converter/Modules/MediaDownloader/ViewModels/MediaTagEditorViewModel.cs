using AutoMapper;
using Bal.Converter.Common.Enums;
using Bal.Converter.Common.Extensions;
using Bal.Converter.Common.Media;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.Services;
using Bal.Converter.YouTubeDl.Quality;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class MediaTagEditorViewModel : ObservableObject, INavigationAware
{
    private readonly IDownloadsRegistryService downloadsRegistry;
    private readonly IMapper mapper;

    [ObservableProperty] private VideoViewModel video;
    [ObservableProperty] private string audioQualityOption;
    [ObservableProperty] private string videoQualityOption;


    public MediaTagEditorViewModel(IDownloadsRegistryService downloadsRegistry, IMapper mapper)
    {
        this.downloadsRegistry = downloadsRegistry;
        this.mapper = mapper;
    }

    public void OnNavigatedTo(object parameter)
    {
        var input = (IDictionary<string, object>)parameter;

        this.Video = input.Get<VideoViewModel>("Video")!;
        this.AudioQualityOption = input.Get<string>("AudioQuality")!;
        this.VideoQualityOption = input.Get<string>("VideoQuality")!;
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private void Download()
    {
        var audioQuality = Enum.Parse<AutomaticQualityOption>(this.AudioQualityOption);
        var videoQuality = Enum.Parse<AutomaticQualityOption>(this.VideoQualityOption);

        var job = new DownloadJob
        {
            Url = this.Video.Url,
            State = DownloadState.Pending,
            Tags = this.mapper.Map<MediaTags>(this.Video.Tags),
            TargetFormat = Enum.Parse<MediaFileExtension>(this.Video.Format)
        };

        this.downloadsRegistry.EnqueueFetch(job);
    }
}