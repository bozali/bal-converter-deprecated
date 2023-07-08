using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Bal.Converter.Modules.Conversion.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bal.Converter.Modules.Conversion.Views;

public sealed partial class ConversionSelectionPage : Page
{
    public ConversionSelectionPage()
    {
        this.ViewModel = App.GetService<ConversionSelectionViewModel>();

        this.InitializeComponent();
    }

    public ConversionSelectionViewModel ViewModel { get; set; }

    private async void OnDrop(object sender, DragEventArgs e)
    {
        if (e.DataView.Contains(StandardDataFormats.StorageItems))
        {
            var items = await e.DataView.GetStorageItemsAsync();

            if (items.Count > 0)
            {
                var file = items.First() as StorageFile;

                if (file != null)
                {
                    this.ViewModel.HandleDrop(file.Path);
                }
            }
        }
    }

    private void OnDragOver(object sender, DragEventArgs e)
    {
        e.AcceptedOperation = DataPackageOperation.Copy;
    }

    private void OnContinueButtonClicked(object sender, RoutedEventArgs e)
    {
        // NOTE This is a workaround since binding to the parent from DataTemplate doesn't work.
        var button = (Button)sender;
        this.ViewModel.ContinueCommand.Execute(button.Tag);
    }
}
