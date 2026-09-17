using System.Collections.ObjectModel;
using System.Windows;
using WPF_SP.Data;
using WPF_SP.Models;

namespace WPF_SP.ViewModels
{
    public class CategoriaViewModel : ViewModelBase
    {
        private readonly CategoriaRepository _repo = new();
        private Categoria _actual = new();
        private Categoria? _seleccionado;

        public ObservableCollection<Categoria> Categorias { get; } = new();

        public Categoria Actual
        {
            get => _actual;
            set => SetProperty(ref _actual, value);
        }

        public Categoria? Seleccionado
        {
            get => _seleccionado;
            set
            {
                if (SetProperty(ref _seleccionado, value) && value is not null)
                {
                    Actual = new Categoria
                    {
                        CategoriaID = value.CategoriaID,
                        NombreCategoria = value.NombreCategoria,
                        Descripcion = value.Descripcion
                    };
                }
            }
        }

        public RelayCommand CargarCommand { get; }
        public RelayCommand NuevoCommand { get; }
        public RelayCommand GuardarCommand { get; }
        public RelayCommand EliminarCommand { get; }

        public CategoriaViewModel()
        {
            CargarCommand = new RelayCommand(_ => Cargar());
            NuevoCommand = new RelayCommand(_ => Nuevo());
            GuardarCommand = new RelayCommand(_ => Guardar());
            EliminarCommand = new RelayCommand(_ => Eliminar(), _ => Seleccionado is not null);

            Cargar();
        }

        private void Cargar()
        {
            Categorias.Clear();
            foreach (var c in _repo.ListarTodos())
                Categorias.Add(c);
        }

        private void Nuevo()
        {
            Seleccionado = null;
            Actual = new Categoria();
        }

        private void Guardar()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Actual.NombreCategoria))
                {
                    MessageBox.Show("El nombre de la categoría es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Actual.CategoriaID == 0)
                    _repo.Insertar(Actual);
                else
                    _repo.Actualizar(Actual);

                Cargar();
                Nuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar la categoría: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Eliminar()
        {
            if (Seleccionado is null) return;

            var confirmar = MessageBox.Show($"¿Eliminar la categoría '{Seleccionado.NombreCategoria}'?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmar != MessageBoxResult.Yes) return;

            try
            {
                _repo.Eliminar(Seleccionado.CategoriaID);
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
