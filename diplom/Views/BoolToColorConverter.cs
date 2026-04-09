namespace diplom.Views;

using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b)
            return b ? Brushes.LightGreen : Brushes.IndianRed;
        return Brushes.LightGray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}