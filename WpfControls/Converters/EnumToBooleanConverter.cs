using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfControls.Converters
{
    /// <summary>
    /// Сравнивает значение с параметром. Может выставлять null значение
    /// </summary>
    public class EnumToBooleanConverter : IValueConverter
    {
        public bool IsNullable { get; set; } = true;

        /// <param name="value">ENUM value</param>
        /// <param name="targetType">ENUM type</param>
        /// <param name="parameter">ENUM benchmark</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // одинаковые ссылки параметра и значения
            if (Equals(value, parameter))
                return true;

            if (Equals(null, parameter) || Equals(value, null))
            {
                if (IsNullable) return null;
                return false;
            }

            try
            {
                // пробую парсить и сравнить
                var result = Enum.Parse(targetType, value.ToString(), true);
                return Equals(result, parameter);
            }
            catch
            {
                // при ошибке возвращаю null
                if (IsNullable) return null;

                return false;
            }

        }


        /// <param name="value">BOOL valuse</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">ENUM behchmakr</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true.Equals(value)
                ? parameter
                : DependencyProperty.UnsetValue
        }

    }
}
