namespace Bal.Converter.Common.Transformation.Attributes ;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TargetAttribute : Attribute
{
    public TargetAttribute(Type target)
    {
        this.Target = target;
    }

    public Type Target { get; set; }
}