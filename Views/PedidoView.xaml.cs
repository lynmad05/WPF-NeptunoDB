using System.Windows;
using System.Windows.Controls;
using WPF_SP.ViewModels;

namespace WPF_SP.Views
{
    public partial class PedidoView : UserControl
    {
        public PedidoView()
        {
            InitializeComponent();
        }

        private async void PedidoView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is PedidoViewModel vm)
                await vm.CargarAsync();
        }
    }
}