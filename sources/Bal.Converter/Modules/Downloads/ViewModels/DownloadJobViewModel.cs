using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Bal.Converter.Modules.Downloads.ViewModels;

public partial class DownloadJobViewModel : ObservableObject
{
    [ObservableProperty] private int id;
    [ObservableProperty] private string url;
    [ObservableProperty] private string title;
    [ObservableProperty] private string progress;
}