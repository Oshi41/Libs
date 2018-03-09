using System;
using System.Windows.Input;

namespace MVVM
{
    public class Command : ICommand
    {
        private readonly Action<object> _action;
        private readonly Predicate<object> _condition;

        public Command(Action<object> action, Predicate<object> condition = null)
        {
            _action = action;
            _condition = condition;
        }

        public Command(Action action, Func<bool> condition = null)
            : this(o => action?.Invoke(), o => condition?.Invoke() ?? true)
        {

        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return _condition?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }

        #endregion

        public void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
