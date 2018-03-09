using System.Linq;
using System.Text.RegularExpressions;

namespace TextHelper
{
    public static class Helper
    {
        /// <summary>
        /// Удаляет пустые строчки в строке
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveEmptyLines(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var result = Regex.Replace(text, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            return result;
        }

        /// <summary>
        /// Оставляю только цифры из строки через пробел
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveNonDights(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;


            return new string(text.Where(char.IsDigit).ToArray());

            //var result = string.Empty;
            //foreach (var c in text)
            //{
            //    if (Char.IsDigit(c))
            //    {
            //        result += c;
            //    }
            //}

            //return result;
            //for (int i = 0; i < text.Length; i++)
            //{
            //    var group = string.Empty;

            //    while (Char.IsDigit(text[i]) 
            //           || Char.IsControl(text[i])
            //           || text[i] == ',' 
            //           || text[i] == '.')
            //    {
            //        group += text[i];
            //    }

            //    // убрал пробелы для парсинга
            //    group = group.Replace(".", ",");

            //    if (Double.TryParse(group, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out var number))
            //    {
            //        list.Add(number);
            //    }
            //}

            //if (!list.Any())
            //    return string.Empty;

            //var result = list.First().ToString();
            //for (int i = 1; i < list.Count; i++)
            //{
            //    result += " " + list[i];
            //}

            //return result;
        }
    }
}
