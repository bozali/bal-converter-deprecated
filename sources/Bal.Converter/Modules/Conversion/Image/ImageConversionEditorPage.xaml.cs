using Windows.Storage.Streams;
using ImageMagick;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Bal.Converter.Modules.Conversion.Image;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ImageConversionEditorPage : Page
{
    public ImageConversionEditorPage()
    {
        this.ViewModel = App.GetService<ImageConversionEditorViewModel>();

        this.InitializeComponent();

        this.ViewModel.SetImageSource = path =>
        {
            using var image = new MagickImage(new FileInfo(path));

            this.ImageControl.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            this.ImageControl.MaxWidth = image.Width;
            this.ImageControl.MaxHeight = image.Height;
            this.ImageControl.Width = image.Width;
            this.ImageControl.Height = image.Height;
        };

        this.ViewModel.UpdateImage = async data =>
        {
            var image = new BitmapImage();

            using var stream = new InMemoryRandomAccessStream();
            using var writer = new DataWriter(stream);

            writer.WriteBytes(data);
            await writer.StoreAsync();
            await writer.FlushAsync();

            writer.DetachStream();

            stream.Seek(0);

            await image.SetSourceAsync(stream);

            this.ImageControl.Source = image;
        };
    }

    public ImageConversionEditorViewModel ViewModel { get; }
}