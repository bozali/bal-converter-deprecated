using System.Web;

using Bal.Converter.Common.Enums;
using Bal.Converter.Contracts.Services;
using Bal.Converter.YouTubeDl.Quality;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bal.Converter.Modules.MediaDownloader.ViewModels;

public partial class MediaDownloaderViewModel : ObservableObject
{
    private readonly INavigationService navigationService;

    [ObservableProperty] private string audioQualityOption;
    [ObservableProperty] private string videoQualityOption;
    [ObservableProperty] private bool proceedAsPlaylist;
    [ObservableProperty] private bool isPlaylist;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPlaylist))]
    [NotifyCanExecuteChangedFor(nameof(EditCommand))]
    [NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
    public string url;

    private string format;

    public MediaDownloaderViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
        this.Url = string.Empty;

        this.AudioQualityOption = AutomaticQualityOption.Best.ToString();
        this.VideoQualityOption = AutomaticQualityOption.Best.ToString();
        this.Format = MediaFileExtension.MP4.ToString();
        this.ProceedAsPlaylist = true;
        this.IsPlaylist = true;
    }

    public string Format
    {
        get => this.format;
        set => this.SetProperty(ref this.format, value);
    }

    [RelayCommand(CanExecute = nameof(CanContinue))]
    private void Convert()
    {
    }

    [RelayCommand(CanExecute = nameof(CanContinue))]
    private void Edit()
    {
        this.navigationService.NavigateTo(typeof(MediaTagEditorViewModel).FullName!);
    }

    private bool CanContinue()
    {
        return !string.IsNullOrEmpty(this.Url);
    }

    partial void OnUrlChanged(string value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            this.IsPlaylist = true;
            return;
        }

        this.IsPlaylist = HttpUtility.ParseQueryString(uri.Query)
                                     .AllKeys
                                     .Any(key => string.Equals(key, "list", StringComparison.OrdinalIgnoreCase));
    }
}