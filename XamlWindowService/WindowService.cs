using System.Windows;
using System.Windows.Media;

namespace XamlWindowService
{
    public class WindowService<T>
        where T : Window, new()
    {
        #region Private fields

        private WindowState _windowState = WindowState.Normal;
        private ImageSource _icon;
        private WindowStartupLocation _location = WindowStartupLocation.Manual;
        
        
        #endregion

        private static WindowService<T> _instance;
        private static WindowService<T> Instance => _instance ?? (_instance = new WindowService<T>());

        /// <summary>
        /// Ограничил выхов конструктора
        /// </summary>
        private WindowService()
        {
            
        }


        private static T GetCustomizedWindow(object viewModel,
            string title = null,
            int width = 0,
            int height = 0,
            bool canResize = true)
        {
            var dlg = new T
            {
                DataContext = viewModel,
                WindowState = Instance._windowState,
                WindowStartupLocation = Instance._location
            };

            if (width > 0)
                dlg.MinWidth = width;
            if (height > 0)
                dlg.MinHeight = height;
            if (!string.IsNullOrEmpty(title))
                dlg.Title = title;
            dlg.ResizeMode = canResize
                ? ResizeMode.CanResize
                : ResizeMode.NoResize;

            if (Instance._icon != null)
                dlg.Icon = Instance._icon;


            return dlg;
        }

        /// <summary>
        /// Показывает модальное окно
        /// </summary>
        /// <param name="viewModel">Модель предстаавления</param>
        /// <param name="title">Заголовок</param>
        /// <param name="width">Минимальная ширина</param>
        /// <param name="height">Минимальная ширина</param>
        /// <param name="canResize">Можем ли изменять размеры окна</param>
        /// <returns></returns>
        public static bool? ShowModal(object viewModel,
            string title = null,
            int width = 0,
            int height = 0,
            bool canResize = true)
        {
            var dlg = GetCustomizedWindow(viewModel, title, width, height, canResize);
            dlg.ShowDialog();

            return dlg.DialogResult;
        }

        #region Customize methods

        public WindowService<T> SetWindowState(WindowState windowState)
        {
            _windowState = windowState;
            return this;
        }

        public WindowService<T> SetIcon(ImageSource icon)
        {
            _icon = icon;
            return this;
        }

        public WindowService<T> SetStartupLocation(WindowStartupLocation location)
        {
            _location = location;
            return this;
        }

        public WindowService<T> UsingMainWindowIcon()
        {
            var icon = Application.Current?.MainWindow?.Icon;
            if (icon != null)
                _icon = icon;
            return this;
        }

        #endregion
    }
}
