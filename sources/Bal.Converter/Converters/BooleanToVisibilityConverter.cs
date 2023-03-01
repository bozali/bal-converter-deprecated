using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Bal.Converter.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if ((bool)value)
        {
            return Visibility.Visible;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is Visibility)
        {
            return (Visibility)value == Visibility.Visible;
        }

        return false;
    }
}