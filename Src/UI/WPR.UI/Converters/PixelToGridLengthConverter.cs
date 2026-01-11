using Avalonia.Data.Converters;
using System;
using Avalonia;
using System.Globalization;

namespace WPR.UI.Converters
{
    public class PixelToGridLengthConverter : IValueConverter
    {
        public static readonly PixelToGridLengthConverter Instance = new PixelToGridLengthConverter();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int i)
            {
                // Avalonia expects GridLength as GridLength struct in Avalonia.Layout
                return new Avalonia.Controls.GridLength(i);
            }

            if (value is double d)
            {
                return new Avalonia.Controls.GridLength((double)d);
            }

            return Avalonia.Controls.GridLength.Auto;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
