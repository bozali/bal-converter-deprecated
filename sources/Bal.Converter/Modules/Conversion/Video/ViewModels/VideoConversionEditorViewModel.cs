using System.Collections.ObjectModel;
using Bal.Converter.Common.Extensions;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.Modules.Conversion.Filters.Views;
using Bal.Converter.Modules.Conversion.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;

namespace Bal.Converter.Modules.Conversion.Video.ViewModels;

public partial class VideoConversionEditorViewModel : ObservableObject, INavigationAware
{
    [ObservableProperty] private VideoConversionOptionsViewModel videoConversionOptions;
    [ObservableProperty] private VideoMetadataViewModel metadata;
    [ObservableProperty] private string? sourcePath;
    [ObservableProperty] private ObservableCollection<Page> filterPages;

    public VideoConversionEditorViewModel()
    {
        this.VideoConversionOptions = new VideoConversionOptionsViewModel();
        this.Metadata = new VideoMetadataViewModel();
    }

    public Action<string> SetMediaPlayerSource { get; set; }

    public void OnNavigatedTo(object parameter)
    {
        var input = (IDictionary<string, object>)parameter;

        this.SourcePath = input.Get<string>("SourcePath");

        this.SetMediaPlayerSource(this.SourcePath);

        this.FilterPages = new ObservableCollection<Page>();
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
}