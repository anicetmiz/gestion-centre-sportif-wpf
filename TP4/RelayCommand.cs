using System.Windows.Input;

namespace Semaine10Exercices
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object?> execute;
        private readonly Predicate<object?>? canExecute;

        public RelayCommand(Action<object?> exec, Predicate<object?>? canExec = null)
        {
            if (exec == null)
                throw new ArgumentNullException("execute");
            execute = exec;
            canExecute = canExec;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}
