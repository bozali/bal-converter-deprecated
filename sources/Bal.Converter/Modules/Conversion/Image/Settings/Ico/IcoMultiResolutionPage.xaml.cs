using Microsoft.UI.Xaml.Controls;

namespace Bal.Converter.Modules.Conversion.Image.Settings.Ico
{
    public sealed partial class IcoMultiResolutionPage : Page
    {
        public IcoMultiResolutionPage()
        {
            this.ViewModel = App.GetService<IcoMultiResolutionViewModel>();

            this.InitializeComponent();
        }

        public IcoMultiResolutionViewModel ViewModel { get; set; }
    }
}
