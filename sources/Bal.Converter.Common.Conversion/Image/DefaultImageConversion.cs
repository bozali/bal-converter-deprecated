using ImageMagick;

namespace Bal.Converter.Common.Conversion.Image;

public abstract class DefaultImageConversion<T> : ConversionBase<T>, IImageConversion where T : IConversion
{
    public override ConversionTopology Topology
    {
        get => ConversionTopology.Image;
    }

    public ImageConversionOptions ImageConversionOptions { get; set; }

    public override async Task Convert(string source, string destination)
    {
        string targetFormat = Path.GetExtension(destination).Replace(".", string.Empty);

        using var image = new MagickImage(this.ImageConversionOptions.UpdatedImageData);
        image.Format = Enum.Parse<MagickFormat>(targetFormat, true);

        await image.WriteAsync(destination).ConfigureAwait(false);
    }
}