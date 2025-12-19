using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace demo_158.Base
{
    class GeneralCommand :ICommand

    {
        private readonly Action _executeAction;
        private readonly Func<bool>? _canExecute;

        public GeneralCommand(Action executeAction,Func<bool>? canExecute = null)
        {
            _executeAction = executeAction;
            _canExecute = canExecute;
        }
        public bool CanExecute(object? parameter)
        {
          return  _canExecute?.Invoke() ?? true;
        }

        public void Execute(object? parameter)
        {
            _executeAction();
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
