using Microsoft.UI.Xaml.Controls;
 
using Bal.Converter.Modules.Conversion.Image.Effects.Watermark;
using ImageMagick;
using Microsoft.UI.Xaml;

namespace Bal.Converter.Modules.Conversion.Image.Effects.Watermark;

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
