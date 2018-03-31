using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfControls.Controls
{
    [TemplatePart(Name = "SearchBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "CancelSearchButton", Type = typeof(Button))]
    [TemplatePart(Name = "SearchButton", Type = typeof(Button))]
    [TemplatePart(Name = "Icon", Type = typeof(Control))]
    public class SearchTextLine : Control
    {
        private readonly ActionArbiter _arbiter = new ActionArbiter();

        #region Dependency property

        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
            "SearchText", typeof(string), typeof(SearchTextLine),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSearchTextChanged));

        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register(
            "SearchCommand", typeof(ICommand), typeof(SearchTextLine),
            new PropertyMetadata(default(ICommand)));


        #endregion

        #region Properties

        public Button SearchButtonPart => Template.FindName("SearchButton", this) as Button;
        public Button CancelSearchButtonPart => Template.FindName("CancelSearchButton", this) as Button;
        public Control IconPart => Template.FindName("Icon", this) as Control;
        public TextBox SearchBoxPart => Template.FindName("SearchBox", this) as TextBox;


        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }
        #endregion

        static SearchTextLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchTextLine),
                new FrameworkPropertyMetadata(typeof(SearchTextLine)));
        }

        public SearchTextLine()
        {
            Loaded += Subscribe;
        }

        private void Subscribe(object sender, RoutedEventArgs e)
        {
            Loaded -= Subscribe;

            SearchBoxPart.TextChanged += OnTextChanged;
            SearchButtonPart.Click += ExecuteSearch;
            CancelSearchButtonPart.Click += CancelSearch;

            KeyUp += TryToSearch;
        }

        #region Executors

        private static void OnSearchTextChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchTextLine textLine)
            {
                textLine._arbiter.Do(() => textLine.SearchBoxPart.Text = e.NewValue?.ToString() ?? string.Empty);
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _arbiter.Do(() => SearchText = SearchBoxPart.Text);
        }

        private void ExecuteSearch(object sender, RoutedEventArgs e)
        {
            SearchCommand?.Execute(SearchBoxPart.Text);
        }
        private void CancelSearch(object sender, RoutedEventArgs e)
        {
            SearchBoxPart.Text = string.Empty;
            ExecuteSearch(sender, e);
        }
        private void TryToSearch(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    ExecuteSearch(null, null);
                    return;

                case Key.Escape:
                    CancelSearch(null, null);
                    break;
            }
        }

        #endregion

    }
}
