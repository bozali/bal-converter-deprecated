using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

using System.Globalization;

namespace Bal.Converter.Converters;

public class BooleanToInvertedVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        bool result = (parameter != null) ? !(bool)value : (bool)value;
        return result ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}