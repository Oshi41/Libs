using System.Windows;

namespace XamlWindowService
{
    public class MessageBoxProvider
    {
        public static bool ShowSuccess(string text, string title = "Success")
        {
            var result = MessageBox.Show(Application.Current.MainWindow,
                text,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            return result == MessageBoxResult.OK;
        }

        public static bool ShowError(string text, string title = "Error")
        {
            var result = MessageBox.Show(Application.Current.MainWindow,
                text,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            return result == MessageBoxResult.OK;
        }

        public static bool Confirm(string question, string title = "Question")
        {
            var result = MessageBox.Show(Application.Current.MainWindow,
                question,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Error);

            return result == MessageBoxResult.Yes;
        }
    }
}
