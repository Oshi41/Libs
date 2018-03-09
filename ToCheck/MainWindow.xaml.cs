using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ToCheck.Annotations;

namespace ToCheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _number;

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public MainWindow()
        {
            InitializeComponent();

            
        }

        public string Number
        {
            get { return _number; }
            set
            {
                if (value == _number) return;
                _number = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
