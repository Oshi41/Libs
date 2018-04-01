using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfControls.Controls.Base;

namespace WpfControls.Controls
{
    [TemplatePart(Name = "SearchBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "CancelSearchButton", Type = typeof(Button))]
    [TemplatePart(Name = "SearchButton", Type = typeof(Button))]
    [TemplatePart(Name = "Icon", Type = typeof(Control))]
    public class SearchTextLine : BaseThematisedControl<SearchTextLine>
    {
        #region Dependency property

        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
            "SearchText", typeof(string), typeof(SearchTextLine),
            new FrameworkPropertyMetadataNew<SearchTextLine>(null,
                OnSearchTextChanged));

        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register(
            "SearchCommand", typeof(ICommand), typeof(SearchTextLine),
            new PropertyMetadata(default(ICommand)));


        #endregion

        #region Properties

        public Button SearchButtonPart => TryFindTemplatePart<Button>("SearchButton");
        public Button CancelSearchButtonPart => TryFindTemplatePart<Button>("CancelSearchButton");
        public Control IconPart => TryFindTemplatePart<Control>("Icon");
        public TextBox SearchBoxPart => TryFindTemplatePart<TextBox>("SearchBox");


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

        protected override void Subscribe()
        {
            base.Subscribe();

            SearchBoxPart.TextChanged += OnTextChanged;
            SearchButtonPart.Click += ExecuteSearch;
            CancelSearchButtonPart.Click += CancelSearch;

            KeyUp += TryToSearch;
        }

        #region Executors

        private static void OnSearchTextChanged(SearchTextLine d,
            DependencyPropertyChangedEventArgs e)
        {
            var text = e.NewValue?.ToString() ?? string.Empty;
            d.ActionArbiter.Do(() => d.SearchBoxPart.Text = text);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //SearchText = SearchBoxPart.Text;
            ActionArbiter.Do(() => SearchText = SearchBoxPart.Text);
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
