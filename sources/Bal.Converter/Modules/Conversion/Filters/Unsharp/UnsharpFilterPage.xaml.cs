using Microsoft.UI.Xaml.Controls;

namespace Bal.Converter.Modules.Conversion.Filters.Unsharp;

public sealed partial class UnsharpFilterPage : Page
{
    public UnsharpFilterPage()
    {
        this.ViewModel = new UnsharpFilterViewModel();

        this.InitializeComponent();
    }

    public UnsharpFilterViewModel ViewModel { get; set; }
}
