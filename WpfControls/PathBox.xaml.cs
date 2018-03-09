using System.Windows;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for PathBox.xaml
    /// </summary>
    public partial class PathBox : UserControl
    {
        public PathBox()
        {
            InitializeComponent();
        }

        #region Dependency prop

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
            "Path", typeof(string), typeof(PathBox),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public static readonly DependencyProperty PathTypeProperty = DependencyProperty.Register(
            "PathType", typeof(PathTypes), typeof(PathBox), 
            new FrameworkPropertyMetadata(PathTypes.Folder, 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public PathTypes PathType
        {
            get { return (PathTypes)GetValue(PathTypeProperty); }
            set { SetValue(PathTypeProperty, value); }
        }

        #endregion

        private void ChooseFolder(object sender, RoutedEventArgs e)
        {
            switch (PathType)
            {
                case PathTypes.Folder:
                    {
                        var dlg = new FolderBrowserDialog();

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            Path = dlg.SelectedPath;
                        }
                    }
                    break;

                case PathTypes.File:
                    {
                        var dlg = new OpenFileDialog();

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            Path = dlg.FileName;
                        }
                    }
                    break;
            }
        }

        #region NEsted

        public enum PathTypes
        {
            Folder,
            File
        }

        #endregion
    }
}
