using Microsoft.UI.Xaml.Controls;

namespace Bal.Converter.Modules.Conversion.Filters.Volume
{
    public sealed partial class VolumeFilterPage : Page
    {
        public VolumeFilterPage()
        {
            this.ViewModel = App.GetService<VolumeFilterViewModel>();

            this.InitializeComponent();
        }

        public VolumeFilterViewModel ViewModel { get; set; }
    }
}
