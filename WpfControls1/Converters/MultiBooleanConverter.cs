using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WpfControls.Converters
{
    /// <summary>
    /// Конвертирует bools в единое значение по типу булевых преобразований. Может быть nullable, по умолчанию именно такой.
    /// </summary>
    public class MultiBooleanConverter : IMultiValueConverter
    {
        /// <summary>
        /// Может ли использовать null значения. Если флажок не выставлен, возвращает false
        /// </summary>
        public bool IsNullable { get; set; } = true;
        /// <summary>
        /// Тип операции конвертора
        /// </summary>
        public OperatorTypes OperatorType { get; set; } = OperatorTypes.AllTrue;


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bools = values.OfType<bool>();

            switch (OperatorType)
            {
                case OperatorTypes.AllTrue:

                    // Всё правильно, выходим
                    if (bools.All(x => x))
                        return true;

                    // Нет ни одного элемента под условие
                    if (bools.All(x => !x))
                        return false;

                    // Пытаемся возвратить null значение
                    if (IsNullable)
                        return null;

                    // выходим
                    return false;

                case OperatorTypes.AllFalse:

                    // Всё правильно, выходим
                    if (bools.All(x => !x))
                        return true;

                    // Нет ни одного элемента под условие
                    if (bools.All(x => x))
                        return false;

                    // Пытаемся возвратить null значение
                    if (IsNullable)
                        return null;

                    // выходим
                    return false;

                case OperatorTypes.AnyTrue:
                    return bools.Any(x => x);

                case OperatorTypes.AnyFalse:
                    return bools.Any(x => !x);


                default:
                    if (IsNullable) return null;
                    return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var count = targetTypes.Length;
            var toFill = OperatorType == OperatorTypes.AllTrue || OperatorType == OperatorTypes.AnyTrue;

            var result = new object[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = toFill;
            }

            return result;
        }

        #region Nested enum

        public enum OperatorTypes
        {
            /// <summary>
            /// AND operator
            /// </summary>
            AllTrue,
            /// <summary>
            /// NOT AND operator
            /// </summary>
            AllFalse,
            /// <summary>
            /// ANY operator
            /// </summary>
            AnyTrue,
            /// <summary>
            /// NOT ANY operator
            /// </summary>
            AnyFalse
        }

        #endregion
    }
}
