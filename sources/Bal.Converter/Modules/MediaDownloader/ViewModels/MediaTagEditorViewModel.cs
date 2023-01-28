using Bal.Converter.Common.Extensions;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.Modules.Downloads;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class MediaTagEditorViewModel : ObservableObject, INavigationAware
{
    [ObservableProperty] private VideoViewModel video;
    [ObservableProperty] private string audioQualityOption;
    [ObservableProperty] private string videoQualityOption;


    public MediaTagEditorViewModel()
    {
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
        var job = new DownloadJob
        {
        };
    }
}