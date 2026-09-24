using System.Collections.ObjectModel;
using System.Windows;
using WPF_SP.Data.Models;
using WPF_SP.Data.Repository;

namespace WPF_SP.ViewModels
{
    public class ProveedorViewModel : ViewModelBase
    {
        private readonly ProveedorRepository _repo = new();
        private Proveedor _actual = new();
        private Proveedor? _seleccionado;
        private string? _filtroContacto;
        private string? _filtroCiudad;

        public ObservableCollection<Proveedor> Proveedores { get; } = new();

        public Proveedor Actual
        {
            get => _actual;
            set => SetProperty(ref _actual, value);
        }

        public Proveedor? Seleccionado
        {
            get => _seleccionado;
            set
            {
                if (SetProperty(ref _seleccionado, value) && value is not null)
                {
                    Actual = new Proveedor
                    {
                        ProveedorID = value.ProveedorID,
                        CompaniaNombre = value.CompaniaNombre,
                        NombreContacto = value.NombreContacto,
                        CargoContacto = value.CargoContacto,
                        Direccion = value.Direccion,
                        Ciudad = value.Ciudad,
                        CodigoPostal = value.CodigoPostal,
                        Pais = value.Pais,
                        Telefono = value.Telefono,
                        Fax = value.Fax
                    };
                }
            }
        }

        public string? FiltroContacto { get => _filtroContacto; set => SetProperty(ref _filtroContacto, value); }
        public string? FiltroCiudad { get => _filtroCiudad; set => SetProperty(ref _filtroCiudad, value); }

        public AsyncRelayCommand CargarCommand { get; }
        public AsyncRelayCommand BuscarCommand { get; }
        public AsyncRelayCommand LimpiarFiltroCommand { get; }
        public RelayCommand NuevoCommand { get; }
        public AsyncRelayCommand GuardarCommand { get; }
        public AsyncRelayCommand EliminarCommand { get; }

        public ProveedorViewModel()
        {
            CargarCommand = new AsyncRelayCommand(_ => CargarAsync());
            BuscarCommand = new AsyncRelayCommand(_ => BuscarAsync());
            LimpiarFiltroCommand = new AsyncRelayCommand(_ => LimpiarFiltroAsync());
            NuevoCommand = new RelayCommand(_ => Nuevo());
            GuardarCommand = new AsyncRelayCommand(_ => GuardarAsync());
            EliminarCommand = new AsyncRelayCommand(_ => EliminarAsync(), _ => Seleccionado is not null);
        }

        public async Task CargarAsync()
        {
            try
            {
                var datos = await _repo.ListarTodosAsync();
                Proveedores.Clear();
                foreach (var p in datos)
                    Proveedores.Add(p);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo conectar a la base de datos:\n{ex.Message}",
                    "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task BuscarAsync()
        {
            try
            {
                var datos = await _repo.BuscarPorContactoYCiudadAsync(FiltroContacto, FiltroCiudad);
                Proveedores.Clear();
                foreach (var p in datos)
                    Proveedores.Add(p);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo buscar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LimpiarFiltroAsync()
        {
            FiltroContacto = null;
            FiltroCiudad = null;
            await CargarAsync();
        }

        private void Nuevo()
        {
            Seleccionado = null;
            Actual = new Proveedor();
        }

        private async Task GuardarAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Actual.CompaniaNombre))
                {
                    MessageBox.Show("La razón social es obligatoria.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Actual.ProveedorID == 0)
                    await _repo.InsertarAsync(Actual);
                else
                    await _repo.ActualizarAsync(Actual);

                await CargarAsync();
                Nuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar el proveedor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task EliminarAsync()
        {
            if (Seleccionado is null) return;

            var confirmar = MessageBox.Show($"¿Eliminar el proveedor '{Seleccionado.CompaniaNombre}'?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmar != MessageBoxResult.Yes) return;

            try
            {
                await _repo.EliminarAsync(Seleccionado.ProveedorID);
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