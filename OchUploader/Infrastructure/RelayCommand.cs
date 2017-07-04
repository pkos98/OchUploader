using System;
using System.Windows.Input;

namespace OchUploader.Infrastructure
{
    public class RelayCommand : ICommand
    {
        public Predicate<object> CanExecuteFunc { get; set; } = x => true;
        public Action ExecuteFunc { get; set; }
        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteFunc.Invoke();
        }

        public event EventHandler CanExecuteChanged;

        public RelayCommand() { }
        public RelayCommand(Action executeFunc)
        {
            ExecuteFunc = executeFunc;
        }
    }
}
