using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace diplom.Views;

public class WidthToCardWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double width)
        {
            if (width < 800) return 300;
            if (width < 1200) return 350;
            return 400;
        }

        return 300;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}