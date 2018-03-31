using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Button = System.Windows.Controls.Button;
using Control = System.Windows.Controls.Control;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using TextBox = System.Windows.Controls.TextBox;

namespace WpfControls.Controls
{
    [TemplatePart(Name = "SearchButton", Type = typeof(Button))]
    [TemplatePart(Name = "Box", Type = typeof(TextBox))]
    public class PathBox : Control
    {
        private readonly ActionArbiter _arbiter = new ActionArbiter();

        #region Dependency property

        public static readonly DependencyProperty IsPathReadOnlyProperty = DependencyProperty.Register(
            "IsPathReadOnly", typeof(bool), typeof(PathBox),
            new FrameworkPropertyMetadata(true, OnEnableChange));

        public static readonly DependencyProperty PathTypesProperty = DependencyProperty.Register(
            "PathType", typeof(PathTypes), typeof(PathBox), new PropertyMetadata(PathTypes.File));


        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
            "Path", typeof(string), typeof(PathBox), 
            new FrameworkPropertyMetadata(null, 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnPathChange));

        public string Path
        {
            get => (string) GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        #endregion

        #region Property

        public PathTypes PathType
        {
            get => (PathTypes)GetValue(PathTypesProperty);
            set => SetValue(PathTypesProperty, value);
        }

        public bool IsPathReadOnly
        {
            get => (bool)GetValue(IsPathReadOnlyProperty);
            set => SetValue(IsPathReadOnlyProperty, value);
        }

        private TextBox BoxPart => (TextBox)Template.FindName("Box", this);
        private Button SearchButtonPart => (Button)Template.FindName("SearchButton", this);

        #endregion

        static PathBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathBox), 
                new FrameworkPropertyMetadata(typeof(PathBox)));
        }

        public PathBox()
        {
            OverridesDefaultStyle = true;

            Loaded += Subscribe;
        }

        private void Subscribe(object sender, RoutedEventArgs e)
        {
            Loaded -= Subscribe;

            SearchButtonPart.Click += ExecuteSearch;
            BoxPart.TextChanged += OnPathChandedInner;

        }

        #region Executors

        private static void OnEnableChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PathBox box
                && e.NewValue is bool enabled
                && box.BoxPart != null)
            {
                box.BoxPart.IsEnabled = enabled;
            }
        }

        private static void OnPathChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PathBox box
                && box.BoxPart != null)
            {
                box._arbiter.Do(() => box.BoxPart.Text = (string) e.NewValue);
            }
        }

        private void OnPathChandedInner(object sender, TextChangedEventArgs e)
        {
            _arbiter.Do(() => Path = BoxPart.Text);
        }

        private void ExecuteSearch(object sender, RoutedEventArgs e)
        {
            switch (PathType)
            {
                case PathTypes.File:
                {
                    var dlg = new OpenFileDialog();
                    if (dlg.ShowDialog() == true)
                    {
                        BoxPart.Text = dlg.FileName;
                    }
                }
                    break;

                case PathTypes.Folder:
                {
                    var dlg = new FolderBrowserDialog();
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        BoxPart.Text = dlg.SelectedPath;
                    }
                }
                    break;
            }
        }

        #endregion

        #region nested enum

        public enum PathTypes
        {
            Folder,
            File
        }

        #endregion
    }
}
