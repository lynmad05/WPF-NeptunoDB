using System.Collections.ObjectModel;
using System.Windows;
using WPF_SP.Data;
using WPF_SP.Models;

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

        public RelayCommand CargarCommand { get; }
        public RelayCommand NuevoCommand { get; }
        public RelayCommand GuardarCommand { get; }
        public RelayCommand EliminarCommand { get; }

        public ProductoViewModel()
        {
            CargarCommand = new RelayCommand(_ => Cargar());
            NuevoCommand = new RelayCommand(_ => Nuevo());
            GuardarCommand = new RelayCommand(_ => Guardar());
            EliminarCommand = new RelayCommand(_ => Eliminar(), _ => Seleccionado is not null);

            Cargar();
        }

        private void Cargar()
        {
            Productos.Clear();
            foreach (var p in _repo.ListarTodos())
                Productos.Add(p);
        }

        private void Nuevo()
        {
            Seleccionado = null;
            Actual = new Producto();
        }

        private void Guardar()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Actual.NombreProducto))
                {
                    MessageBox.Show("El nombre del producto es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Actual.ProductoID == 0)
                    _repo.Insertar(Actual);
                else
                    _repo.Actualizar(Actual);

                Cargar();
                Nuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar el producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Eliminar()
        {
            if (Seleccionado is null) return;

            var confirmar = MessageBox.Show($"¿Eliminar el producto '{Seleccionado.NombreProducto}'?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmar != MessageBoxResult.Yes) return;

            try
            {
                _repo.Eliminar(Seleccionado.ProductoID);
                Cargar();
                Nuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo eliminar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
