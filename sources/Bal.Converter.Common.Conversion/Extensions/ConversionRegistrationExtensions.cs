using Bal.Converter.Common.Conversion.Audio;
using Bal.Converter.Common.Conversion.Image;

using Microsoft.Extensions.DependencyInjection;

namespace Bal.Converter.Common.Conversion.Extensions;

public static class ConversionRegistrationExtensions
{
    public static IServiceCollection ConfigureConversions(this IServiceCollection collection)
    {
        collection
            .AddTransient<Mp3Conversion>()
            
            // Image
            .AddTransient<PngConversion>()
            .AddTransient<BmpConversion>()
            .AddTransient<JpegConversion>()
            .AddTransient<GifConversion>()
            .AddTransient<IcoConversion>();

        return collection;
    }
}