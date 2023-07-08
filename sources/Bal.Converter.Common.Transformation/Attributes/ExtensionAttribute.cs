namespace Bal.Converter.Common.Transformation.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ExtensionAttribute : Attribute
{
    public ExtensionAttribute(string extension)
    {
        this.Extension = extension;
    }

    public string Extension { get; set; }
}