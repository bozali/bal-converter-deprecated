using CommunityToolkit.Mvvm.ComponentModel;

namespace Bal.Converter.Modules.Conversion.Filters.ViewModels ;

public abstract partial class FilterBaseViewModel : ObservableObject
{
    [ObservableProperty] private string displayName;

    protected FilterBaseViewModel(string displayName)
    {
        this.DisplayName = displayName;
    }
}