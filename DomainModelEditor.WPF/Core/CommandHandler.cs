using System;
using System.Windows.Input;

namespace DomainModelEditor
{
    public class CommandHandler : ICommand
    {
        private readonly Action _TargetExecuteMethod;
        private readonly Func<bool> _TargetCanExecuteMethod;

        public CommandHandler(Action executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public CommandHandler(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
            {
                return _TargetCanExecuteMethod.Invoke();
            }
            if (_TargetExecuteMethod != null)
            {
                return true;
            }
            return false;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        void ICommand.Execute(object parameter)
        {
            if (_TargetExecuteMethod != null)
            {
                _TargetExecuteMethod();
            }
        }
        #endregion
    }

    public class RelayCommand<T> : ICommand
    {
        Action<T> _TargetExecuteMethod;
        Func<T, bool> _TargetCanExecuteMethod;

        public RelayCommand(Action<T> executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action<T> executeMethod, Func<T,bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged() 
        {
             CanExecuteChanged(this, EventArgs.Empty); 
        }
        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
            {
                T tparm = (T)parameter;
                return _TargetCanExecuteMethod(tparm);
            }
            if (_TargetExecuteMethod != null)
            {
                return true;
            }
            return false;
        }
        public event EventHandler CanExecuteChanged = delegate { };

        void ICommand.Execute(object parameter)
        {
            if (_TargetExecuteMethod != null)
            {
                _TargetExecuteMethod((T)parameter);
            }
        }
        #endregion
    }
}
