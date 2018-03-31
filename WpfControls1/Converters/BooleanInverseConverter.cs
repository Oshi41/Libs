using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfControls.Converters
{
    /// <summary>
    /// Инвертирует bool значение. Может работать с null значением
    /// </summary>
    public class BooleanInverseConverter : IValueConverter
    {
        public bool IsNullable { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Пришёл ноль, конвертируем
            if (ReferenceEquals(null, value))
            {
                if (IsNullable)
                    return null;

                return false;
            }

            // value имеет bool тип
            if (value is bool val)
            {
                return !val;
            }

            // в остальных случаях ничего не делаем
            return DependencyProperty.UnsetValue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
