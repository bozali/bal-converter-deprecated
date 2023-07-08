namespace Bal.Converter.Common.Transformation;


public interface IFileTransformation
{
    TransformationTopology Topology { get; }

    string Extension { get; }

    Type[] SupportedTargets { get; }

    Task Transform(string source, string destination);
}
