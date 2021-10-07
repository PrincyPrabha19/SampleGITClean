using System;
using System.Windows.Input;

namespace Dominator.UI.Classes.Helpers
{
    public class RelayCommand<T> : ICommand
    {
        #region Fields
        private readonly Action<T> execute;
        private readonly Predicate<T> canExecute;
        #endregion

        #region Constructors
        public RelayCommand()
        {
        }

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        #endregion

        #region ICommand Members
        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke((T)parameter) ?? false;
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
        
        #endregion
    }
}
