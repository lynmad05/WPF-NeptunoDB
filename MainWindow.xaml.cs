using System.Windows;
using WPF_SP.Views;

namespace WPF_SP
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContenidoPrincipal.Content = new ProductoView();
        }

        private void BtnProductos_Click(object sender, RoutedEventArgs e)
            => ContenidoPrincipal.Content = new ProductoView();

        private void BtnCategorias_Click(object sender, RoutedEventArgs e)
            => ContenidoPrincipal.Content = new CategoriaView();

        private void BtnProveedores_Click(object sender, RoutedEventArgs e)
            => ContenidoPrincipal.Content = new ProveedorView();

        private void BtnPedidos_Click(object sender, RoutedEventArgs e)
            => ContenidoPrincipal.Content = new PedidoView();
    }
}
