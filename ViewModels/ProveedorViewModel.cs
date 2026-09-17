using System.Collections.ObjectModel;
using System.Windows;
using WPF_SP.Data;
using WPF_SP.Models;

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

        public string? FiltroContacto
        {
            get => _filtroContacto;
            set => SetProperty(ref _filtroContacto, value);
        }

        public string? FiltroCiudad
        {
            get => _filtroCiudad;
            set => SetProperty(ref _filtroCiudad, value);
        }

        public RelayCommand CargarCommand { get; }
        public RelayCommand BuscarCommand { get; }
        public RelayCommand LimpiarFiltroCommand { get; }
        public RelayCommand NuevoCommand { get; }
        public RelayCommand GuardarCommand { get; }
        public RelayCommand EliminarCommand { get; }

        public ProveedorViewModel()
        {
            CargarCommand = new RelayCommand(_ => Cargar());
            BuscarCommand = new RelayCommand(_ => Buscar());
            LimpiarFiltroCommand = new RelayCommand(_ => LimpiarFiltro());
            NuevoCommand = new RelayCommand(_ => Nuevo());
            GuardarCommand = new RelayCommand(_ => Guardar());
            EliminarCommand = new RelayCommand(_ => Eliminar(), _ => Seleccionado is not null);

            Cargar();
        }

        private void Cargar()
        {
            Proveedores.Clear();
            foreach (var p in _repo.ListarTodos())
                Proveedores.Add(p);
        }

        private void Buscar()
        {
            Proveedores.Clear();
            foreach (var p in _repo.BuscarPorContactoYCiudad(FiltroContacto, FiltroCiudad))
                Proveedores.Add(p);
        }

        private void LimpiarFiltro()
        {
            FiltroContacto = null;
            FiltroCiudad = null;
            Cargar();
        }

        private void Nuevo()
        {
            Seleccionado = null;
            Actual = new Proveedor();
        }

        private void Guardar()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Actual.CompaniaNombre))
                {
                    MessageBox.Show("La razón social es obligatoria.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Actual.ProveedorID == 0)
                    _repo.Insertar(Actual);
                else
                    _repo.Actualizar(Actual);

                Cargar();
                Nuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar el proveedor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Eliminar()
        {
            if (Seleccionado is null) return;

            var confirmar = MessageBox.Show($"¿Eliminar el proveedor '{Seleccionado.CompaniaNombre}'?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmar != MessageBoxResult.Yes) return;

            try
            {
                _repo.Eliminar(Seleccionado.ProveedorID);
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
