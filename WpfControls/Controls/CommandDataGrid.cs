using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfControls.Controls
{
    /// <summary>
    /// DataGrid с коммандами на строку
    /// </summary>
    public class CommandDataGrid : DataGrid
    {
        #region Properties

        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.Register(
            "DoubleClickCommand", typeof(ICommand), typeof(CommandDataGrid), 
            new PropertyMetadata(default(ICommand)));

        public ICommand DoubleClickCommand
        {
            get => (ICommand) GetValue(DoubleClickCommandProperty);
            set => SetValue(DoubleClickCommandProperty, value);
        }

        public static readonly DependencyProperty LeftClickCommandProperty = DependencyProperty.Register(
            "LeftClickCommand", typeof(ICommand), typeof(CommandDataGrid), 
            new PropertyMetadata(default(ICommand)));

        public ICommand LeftClickCommand
        {
            get => (ICommand) GetValue(LeftClickCommandProperty);
            set => SetValue(LeftClickCommandProperty, value);
        }

        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(
            "ClickCommand", typeof(ICommand), typeof(CommandDataGrid), 
            new PropertyMetadata(default(ICommand)));

        public ICommand ClickCommand
        {
            get => (ICommand) GetValue(ClickCommandProperty);
            set => SetValue(ClickCommandProperty, value);
        }

        #endregion

        protected override void OnLoadingRow(DataGridRowEventArgs e)
        {
            base.OnLoadingRow(e);

            e.Row.MouseDoubleClick += ExecuteMouseDoubleClickCommand;
            e.Row.MouseLeftButtonUp += ExecuteMouseClickCommand;
            e.Row.MouseRightButtonUp += ExecuteMouseLeftButtonCommand;
        }
        protected override void OnUnloadingRow(DataGridRowEventArgs e)
        {
            base.OnUnloadingRow(e);

            e.Row.MouseDoubleClick -= ExecuteMouseDoubleClickCommand;
            e.Row.MouseLeftButtonUp -= ExecuteMouseClickCommand;
            e.Row.MouseRightButtonUp -= ExecuteMouseLeftButtonCommand;
        }


        private void ExecuteMouseLeftButtonCommand(object sender, MouseButtonEventArgs e)
        {
            LeftClickCommand?.Execute(sender);
        }
        private void ExecuteMouseClickCommand(object sender, MouseButtonEventArgs e)
        {
            ClickCommand?.Execute(sender);
        }
        private void ExecuteMouseDoubleClickCommand(object sender, MouseButtonEventArgs e)
        {
            DoubleClickCommand?.Execute(sender);
        }
    }
}
