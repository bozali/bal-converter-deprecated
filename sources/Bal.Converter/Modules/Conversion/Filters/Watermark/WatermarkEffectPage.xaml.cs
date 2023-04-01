using ImageMagick;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bal.Converter.Modules.Conversion.Filters.Watermark;

public sealed partial class WatermarkEffectPage : Page
{
    public WatermarkEffectPage()
    {
        this.ViewModel = App.GetService<WatermarkEffectViewModel>();

        this.InitializeComponent();
    }

    public WatermarkEffectViewModel ViewModel { get; set; }

    private void OnGravityComboboxLoaded(object sender, RoutedEventArgs e)
    {
        // NOTE This is a workaround, somehow comboboxes are not set to an initial value in UI
        ((ComboBox)sender).SelectedValue = Gravity.Center;
        ((ComboBox)sender).SelectedIndex = 4;
    }
}
