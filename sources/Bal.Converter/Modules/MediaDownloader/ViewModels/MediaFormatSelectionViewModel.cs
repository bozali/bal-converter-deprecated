using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public class MediaFormatSelectionViewModel : ObservableObject, INavigationAware
{
    private readonly ILogger<MediaFormatSelectionViewModel> logger;
    private readonly INavigationService navigationService;

    public MediaFormatSelectionViewModel(ILogger<MediaFormatSelectionViewModel> logger,
                                         INavigationService navigationService)
    {
        this.logger = logger;
        this.navigationService = navigationService;
    }

    public void OnNavigatedTo(object parameter)
    {
    }

    public void OnNavigatedFrom()
    {
    }
}