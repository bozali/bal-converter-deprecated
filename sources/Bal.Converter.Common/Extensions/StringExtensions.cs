namespace Bal.Converter.Common.Extensions;

public static class StringExtensions
{
    public static string RemoveIllegalChars(this string str)
    {
        string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

        foreach (char c in invalid)
        {
            str = str.Replace(c.ToString(), "");
        }

        return str;
    }

    public static string ToTitleCase(this string str)
    {
        return str.Length switch
        {
            0 => string.Empty,
            1 => str.ToUpper(),
            _ => char.ToUpper(str[0]) + str[1..]
        };
    }
}