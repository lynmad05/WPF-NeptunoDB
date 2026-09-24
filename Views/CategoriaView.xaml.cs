using System.Windows;
using System.Windows.Controls;
using WPF_SP.ViewModels;

namespace WPF_SP.Views
{
    public partial class CategoriaView : UserControl
    {
        public CategoriaView()
        {
            InitializeComponent();
        }

        private async void CategoriaView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CategoriaViewModel vm)
                await vm.CargarAsync();
        }
    }
}