using Bal.Converter.Contracts.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public class PlaylistOverviewViewModel : ObservableObject, INavigationAware
{
    public PlaylistOverviewViewModel()
    {
    }

    public void OnNavigatedTo(object parameter)
    {
        var input = (IDictionary<string, object>)parameter;
    }

    public void OnNavigatedFrom()
    {
    }
}