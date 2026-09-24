using System.Windows;
using System.Windows.Controls;
using WPF_SP.ViewModels;

namespace WPF_SP.Views
{
    public partial class ProveedorView : UserControl
    {
        public ProveedorView()
        {
            InitializeComponent();
        }

        private async void ProveedorView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProveedorViewModel vm)
                await vm.CargarAsync();
        }
    }
}