using ImageMagick;

namespace Bal.Converter.Common.Transformation.Image;

public abstract class DefaultImageTransformation<T> : FileTransformerBase<T>, IImageTransformation where T : IFileTransformation
{
    public override TransformationTopology Topology
    {
        get => TransformationTopology.Image;
    }

    public ImageTransformationOptions ImageTransformationOptions { get; set; }

    public override async Task Transform(string source, string destination)
    {
        string targetFormat = Path.GetExtension(destination).Replace(".", string.Empty);

        using var image = new MagickImage(this.ImageTransformationOptions.UpdatedImageData);
        image.Format = Enum.Parse<MagickFormat>(targetFormat, true);

        await image.WriteAsync(destination).ConfigureAwait(false);
    }
}