using System;
using System.Windows.Input;

namespace MaterialStorageManager.Utils
{
    public class Command : ICommand
    {
        Action<object> _executeMethod;
        Func<object, bool> _canexecuteMethod;
        string Name = string.Empty;

        public Command(Action<object> executeMethod) : this(executeMethod, null)
        {

        }

        public Command(Action<object> executeMethod, Func<object, bool> canexecuteMethod)
        {
            this._executeMethod = executeMethod;
            this._canexecuteMethod = canexecuteMethod;
        }

        public Command()
        {

        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeMethod(parameter);
        }
    }
}
