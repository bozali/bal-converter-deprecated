using Bal.Converter.Common.Transformation.Audio;
using Bal.Converter.Common.Transformation.Image;
using Bal.Converter.Common.Transformation.Video;
using Microsoft.Extensions.DependencyInjection;

namespace Bal.Converter.Common.Transformation.Extensions;

public static class TransformationRegistrationExtensions
{
    public static IServiceCollection ConfigureTransformation(this IServiceCollection collection)
    {
        collection
            // Audio
            .AddTransient<Mp3Transformation>()

            // Video
            .AddTransient<Mp4Transformation>()
            .AddTransient<WavTransformation>()
            .AddTransient<AviTransformation>()
            .AddTransient<MkvTransformation>()
            
            // Image
            .AddTransient<PngTransformation>()
            .AddTransient<BmpTransformation>()
            .AddTransient<JpegTransformation>()
            .AddTransient<GifTransformation>()
            .AddTransient<IcoTransformation>();

        return collection;
    }
}