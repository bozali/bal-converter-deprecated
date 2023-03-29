namespace Bal.Converter.Common.Conversion.Image;

public interface IImageConversion : IConversion
{
    ImageConversionOptions ImageConversionOptions { get; set; }
}