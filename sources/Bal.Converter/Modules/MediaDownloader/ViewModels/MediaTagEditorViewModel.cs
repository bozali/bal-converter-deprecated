using Bal.Converter.Contracts.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class MediaTagEditorViewModel : ObservableObject, INavigationAware
{
    [ObservableProperty] private VideoViewModel video;

    public MediaTagEditorViewModel()
    {
    }

    public void OnNavigatedTo(object parameter)
    {
        this.Video = (VideoViewModel)parameter;
    }

    public void OnNavigatedFrom()
    {
    }
}