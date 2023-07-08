namespace Bal.Converter.Common.Transformation;

/// <summary>
/// Depending on which topology is supported we can use a converter, its functionality and options.
/// </summary>
[Flags]
public enum TransformationTopology
{
    None = 0,
    Audio = 1 << 0,
    Document = 1 << 1,
    Video = 1 << 2,
    Image = 1 << 3,
}