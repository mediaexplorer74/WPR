using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WPR
{
    class DateTimeConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            return ((DateTime)value).ToString("HH:mm");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
