using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfControls.Converters
{
    /// <summary>
    /// Преобразует bool в видимость контрола. Может коллапсировать контрол, так и скрывать его
    /// </summary>
    class InverseBooleanToVisibilityConverter : IValueConverter 
    {
        public bool MakeHidden { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (false.Equals(value))
            {
                return Visibility.Visible;
            }

            return MakeHidden
                ? Visibility.Hidden
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !Visibility.Visible.Equals(value);
        }
    }
}
