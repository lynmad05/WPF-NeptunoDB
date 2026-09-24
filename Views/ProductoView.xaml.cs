using System.Windows;
using System.Windows.Controls;
using WPF_SP.ViewModels;

namespace WPF_SP.Views
{
    public partial class ProductoView : UserControl
    {
        public ProductoView()
        {
            InitializeComponent();
        }

        private async void ProductoView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductoViewModel vm)
                await vm.CargarAsync();
        }
    }
}