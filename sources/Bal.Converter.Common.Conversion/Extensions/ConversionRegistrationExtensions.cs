using Bal.Converter.Common.Conversion.Audio;

using Microsoft.Extensions.DependencyInjection;

namespace Bal.Converter.Common.Conversion.Extensions;

public static class ConversionRegistrationExtensions
{
    public static IServiceCollection ConfigureConversions(this IServiceCollection collection)
    {
        collection.AddTransient<Mp3Conversion>();

        return collection;
    }
}