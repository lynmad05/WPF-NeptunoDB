using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_SP.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T campo, T valor, [CallerMemberName] string? nombre = null)
        {
            if (Equals(campo, valor)) return false;
            campo = valor;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
            return true;
        }
    }
}
