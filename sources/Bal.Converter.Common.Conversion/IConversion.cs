namespace Bal.Converter.Common.Conversion;


public interface IConversion
{
    ConversionTopology Topology { get; }

    string Extension { get; }

    Type[] SupportedTargets { get; }

    Task Convert(string source, string destination);
}
