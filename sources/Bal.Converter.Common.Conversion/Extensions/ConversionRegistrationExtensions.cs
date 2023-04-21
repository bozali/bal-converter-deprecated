using Bal.Converter.Common.Conversion.Audio;
using Bal.Converter.Common.Conversion.Image;
using Bal.Converter.Common.Conversion.Video;
using Microsoft.Extensions.DependencyInjection;

namespace Bal.Converter.Common.Conversion.Extensions;

public static class ConversionRegistrationExtensions
{
    public static IServiceCollection ConfigureConversions(this IServiceCollection collection)
    {
        collection
            // Audio
            .AddTransient<Mp3Conversion>()

            // Video
            .AddTransient<Mp4Conversion>()
            .AddTransient<WavConversion>()
            .AddTransient<AviConversion>()
            .AddTransient<MkvConversion>()
            
            // Image
            .AddTransient<PngConversion>()
            .AddTransient<BmpConversion>()
            .AddTransient<JpegConversion>()
            .AddTransient<GifConversion>()
            .AddTransient<IcoConversion>();

        return collection;
    }
}