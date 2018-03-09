using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM
{
    internal class AsyncCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
        private bool _isExecuting;

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
            : this((o) => execute(), o => canExecute?.Invoke() ?? true)
        {
            
        }

        public AsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = o => canExecute?.Invoke(o) ?? true;
        }

        public bool CanExecute(object parameter)
        {
            return !(_isExecuting && _canExecute(parameter));
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public async void Execute(object parameter)
        {
            _isExecuting = true;
            OnCanExecuteChanged();
            try
            {
                await _execute(parameter);
            }
            catch 
            {
                //ignored
            }
            finally
            {
                _isExecuting = false;
                OnCanExecuteChanged();
            }
        }

        protected virtual void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
