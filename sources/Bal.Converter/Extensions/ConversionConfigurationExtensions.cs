using Bal.Converter.Modules.Conversion.Filters.Unsharp;
using Bal.Converter.Modules.Conversion.Filters.Volume;
using Bal.Converter.Modules.Conversion.Filters.Watermark;
using Bal.Converter.Modules.Conversion.Image.Settings.Ico;

using Microsoft.Extensions.DependencyInjection;

namespace Bal.Converter.Extensions;

public static class ConversionConfigurationExtensions
{
    public static IServiceCollection ConfigureConversionViews(this IServiceCollection collection)
    {
        collection
            // Views
            .AddTransient<VolumeFilterPage>()
            .AddTransient<WatermarkEffectPage>()
            .AddTransient<IcoMultiResolutionPage>()
            .AddTransient<UnsharpFilterPage>()

            // ViewModels
            .AddTransient<VolumeFilterViewModel>()
            .AddTransient<WatermarkEffectViewModel>()
            .AddTransient<IcoMultiResolutionViewModel>()
            .AddTransient<UnsharpFilterViewModel>();

        return collection;
    }
}