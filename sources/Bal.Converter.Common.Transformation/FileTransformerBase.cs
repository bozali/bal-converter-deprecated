namespace Bal.Converter.Common.Transformation;

public abstract class FileTransformerBase<T> : IFileTransformation where T : IFileTransformation
{
    protected FileTransformerBase()
    {
        this.SupportedTargets = TransformationAttributeExtractor.ExtractSupportedTypes(typeof(T));
        this.Extension = TransformationAttributeExtractor.ExtractExtension(typeof(T));
    }

    public abstract TransformationTopology Topology { get; }

    public string Extension { get; }

    public Type[] SupportedTargets { get; }

    public abstract Task Transform(string source, string destination);
}