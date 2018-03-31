using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfControls.Converters
{
    /// <summary>
    /// Преобразует bool в видимость контрола. Может коллапсировать контрол, так и скрывать его
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool MakeHidden { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (true.Equals(value))
            {
                return Visibility.Visible;
            }

            return MakeHidden
                ? Visibility.Hidden
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Visibility.Visible.Equals(value);
        }
    }
}
