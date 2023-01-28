namespace Bal.Converter.Common.Extensions ;

public static class CollectionExtensions
{
    public static T? Get<T>(this IDictionary<string, object> dictionary, string key)
    {
        try
        {
            return (T)dictionary[key];
        }
        catch (Exception e)
        {
            return default;
        }
    }
}