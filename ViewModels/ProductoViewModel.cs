using System.Collections.ObjectModel;
using System.Windows;
using WPF_SP.Data.Models;
using WPF_SP.Data.Repository;

namespace WPF_SP.ViewModels
{
    public class ProductoViewModel : ViewModelBase
    {
        private readonly ProductoRepository _repo = new();
        private Producto _actual = new();
        private Producto? _seleccionado;

        public ObservableCollection<Producto> Productos { get; } = new();

        public Producto Actual
        {
            get => _actual;
            set => SetProperty(ref _actual, value);
        }

        public Producto? Seleccionado
        {
            get => _seleccionado;
            set
            {
                if (SetProperty(ref _seleccionado, value) && value is not null)
                {
                    Actual = new Producto
                    {
                        ProductoID = value.ProductoID,
                        NombreProducto = value.NombreProducto,
                        ProveedorID = value.ProveedorID,
                        CategoriaID = value.CategoriaID,
                        CantidadPorUnidad = value.CantidadPorUnidad,
                        PrecioUnidad = value.PrecioUnidad,
                        UnidadesEnExistencia = value.UnidadesEnExistencia,
                        UnidadesEnPedido = value.UnidadesEnPedido,
                        NivelDeReorden = value.NivelDeReorden,
                        Descontinuado = value.Descontinuado
                    };
                }
            }
        }

        public AsyncRelayCommand CargarCommand { get; }
        public RelayCommand NuevoCommand { get; }
        public AsyncRelayCommand GuardarCommand { get; }
        public AsyncRelayCommand EliminarCommand { get; }

        public ProductoViewModel()
        {
            CargarCommand = new AsyncRelayCommand(_ => CargarAsync());
            NuevoCommand = new RelayCommand(_ => Nuevo());
            GuardarCommand = new AsyncRelayCommand(_ => GuardarAsync());
            EliminarCommand = new AsyncRelayCommand(_ => EliminarAsync(), _ => Seleccionado is not null);

            // OJO: aquí YA NO se llama a CargarAsync().
            // Un constructor no puede ser async, así que la carga inicial
            // se dispara desde el evento Loaded de la Vista (ver ProductoView.xaml.cs).
        }

        public async Task CargarAsync()
        {
            try
            {
                var datos = await _repo.ListarTodosAsync();
                Productos.Clear();
                foreach (var p in datos)
                    Productos.Add(p);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo conectar a la base de datos:\n{ex.Message}",
                    "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Nuevo()
        {
            Seleccionado = null;
            Actual = new Producto();
        }

        private async Task GuardarAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Actual.NombreProducto))
                {
                    MessageBox.Show("El nombre del producto es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Actual.ProductoID == 0)
                    await _repo.InsertarAsync(Actual);
                else
                    await _repo.ActualizarAsync(Actual);

                await CargarAsync();
                Nuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar el producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task EliminarAsync()
        {
            if (Seleccionado is null) return;

            var confirmar = MessageBox.Show($"¿Eliminar el producto '{Seleccionado.NombreProducto}'?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmar != MessageBoxResult.Yes) return;

            try
            {
                await _repo.EliminarAsync(Seleccionado.ProductoID);
                await CargarAsync();
                Nuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo eliminar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}