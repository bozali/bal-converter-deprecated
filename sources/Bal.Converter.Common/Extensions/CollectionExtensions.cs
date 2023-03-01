using System.Collections.ObjectModel;

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

    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }
}