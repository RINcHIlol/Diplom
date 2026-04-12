namespace diplom.Services;

using System;
using System.Globalization;
using Avalonia.Data.Converters;

public class BoolToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var parts = parameter?.ToString()?.Split('|');

        if (value is bool b && parts?.Length == 2)
            return b ? parts[0] : parts[1];

        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}