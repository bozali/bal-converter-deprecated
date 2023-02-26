using Bal.Converter.Common.Conversion.Attributes;

namespace Bal.Converter.Common.Conversion;

public static class ConversionAttributeExtractor
{
    public static string ExtractExtension(Type type)
    {

        if (!type.GetInterfaces().Contains(typeof(IConversion)))
        {
            throw new ArgumentException(nameof(type));
        }

        if (Attribute.GetCustomAttribute(type, typeof(ExtensionAttribute)) is not ExtensionAttribute extensionAttribute)
        {
            throw new Exception($"Extension is missing for {type.FullName}.");
        }

        return extensionAttribute.Extension;
    }

    public static Type[] ExtractSupportedTypes(Type type)
    {
        var targetAttributes = Attribute.GetCustomAttributes(type, typeof(TargetAttribute)) as TargetAttribute[];

        if (targetAttributes == null || !targetAttributes.Any())
        {
            throw new ArgumentException($"Target is missing for {type.FullName}.");
        }

        return targetAttributes.Select(t => t.Target).ToArray();
    }
}