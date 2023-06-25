using System.Diagnostics;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.About.ViewModels;

public partial class AboutViewModel : ObservableObject
{
    [ObservableProperty] private string appNameVersion;
    [ObservableProperty] private string copyright;

    public AboutViewModel()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

        this.Copyright = $"\u00a9 {DateTime.Now.Year} Ali Bozna. All rights reserved.";
        this.AppNameVersion = $"Bal Converter {versionInfo.FileVersion + " Preview" ?? "1.0"}";
    }
}