using System.Windows.Input;

namespace WPF_SP.ViewModels
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object?, Task> _execute;
        private readonly Func<object?, bool>? _canExecute;
        private bool _ejecutando;

        public AsyncRelayCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => !_ejecutando && (_canExecute?.Invoke(parameter) ?? true);

        public async void Execute(object? parameter)
        {
            _ejecutando = true;
            RaiseCanExecuteChanged();
            try
            {
                await _execute(parameter);
            }
            finally
            {
                _ejecutando = false;
                RaiseCanExecuteChanged();
            }
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}