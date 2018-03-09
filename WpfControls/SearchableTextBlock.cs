using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfControls
{
    public class SearchableTextBlock : TextBlock
    {
        #region Dependency property

        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
            "SearchText", typeof(string), typeof(SearchableTextBlock),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                HighLightText));

        private static void HighLightText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // обработка ошибок
            if (!(d is SearchableTextBlock block)) return;

            var text = block.Text;
            var searchText = block.SearchText;

            // негде искать
            if (string.IsNullOrEmpty(text)) return;
            
            // пустой поиск, обнуляем селект
            if (string.IsNullOrEmpty(searchText))
            {
                block.CancelSelect();
                return;
            }

            // очищаем текстблок
            block.Inlines.Clear();
            // ищу по regex
            var regex = new Regex(Regex.Escape(searchText));
            var splitted = regex.Split(text);

            // Ничего не найдено
            if (splitted.Length < 1)
            {
                block.Text = text;
            }
            else
            {
                // добавляю первую строчку
                block.Inlines.Add(splitted[0]);
                // потом добавляю остальные через разделитель - строку поиска
                foreach (var s in splitted.Skip(1))
                {
                    block.Inlines.Add(block.GetSelectedRun(searchText));
                    block.Inlines.Add(s);
                }
            }
        }

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty SelectionBrushProperty = DependencyProperty.Register(
            "SelectionBrush", typeof(Brush), typeof(SearchableTextBlock), 
            new PropertyMetadata(Brushes.Yellow));

        public Brush SelectionBrush
        {
            get { return (Brush) GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        #endregion

        private void CancelSelect()
        {
            var temp = Text;
            Inlines.Clear();
            Text = temp;
        }

        private Run GetSelectedRun(string text)
        {
            return new Run(text)
            {
                Background = SelectionBrush,
                FontWeight = FontWeights.Bold,
            };
        }
    }
}
