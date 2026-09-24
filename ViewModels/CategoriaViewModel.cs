using System.Collections.ObjectModel;
using System.Windows;
using WPF_SP.Data.Models;
using WPF_SP.Data.Repository;

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

        public AsyncRelayCommand CargarCommand { get; }
        public RelayCommand NuevoCommand { get; }
        public AsyncRelayCommand GuardarCommand { get; }
        public AsyncRelayCommand EliminarCommand { get; }

        public CategoriaViewModel()
        {
            CargarCommand = new AsyncRelayCommand(_ => CargarAsync());
            NuevoCommand = new RelayCommand(_ => Nuevo());
            GuardarCommand = new AsyncRelayCommand(_ => GuardarAsync());
            EliminarCommand = new AsyncRelayCommand(_ => EliminarAsync(), _ => Seleccionado is not null);
        }

        public async Task CargarAsync()
        {
            try
            {
                var datos = await _repo.ListarTodosAsync();
                Categorias.Clear();
                foreach (var c in datos)
                    Categorias.Add(c);
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
            Actual = new Categoria();
        }

        private async Task GuardarAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Actual.NombreCategoria))
                {
                    MessageBox.Show("El nombre de la categoría es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Actual.CategoriaID == 0)
                    await _repo.InsertarAsync(Actual);
                else
                    await _repo.ActualizarAsync(Actual);

                await CargarAsync();
                Nuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar la categoría: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task EliminarAsync()
        {
            if (Seleccionado is null) return;

            var confirmar = MessageBox.Show($"¿Eliminar la categoría '{Seleccionado.NombreCategoria}'?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmar != MessageBoxResult.Yes) return;

            try
            {
                await _repo.EliminarAsync(Seleccionado.CategoriaID);
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