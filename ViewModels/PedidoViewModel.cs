using System.Collections.ObjectModel;
using System.Windows;
using WPF_SP.Data.Models;
using WPF_SP.Data.Repository;

namespace WPF_SP.ViewModels
{
    public class PedidoViewModel : ViewModelBase
    {
        private readonly PedidoRepository _repo = new();
        private Pedido _actual = new();
        private Pedido? _seleccionado;
        private DateTime _fechaInicioReporte = DateTime.Today.AddDays(-30);
        private DateTime _fechaFinReporte = DateTime.Today;
        private decimal _totalReporte;

        public ObservableCollection<Pedido> Pedidos { get; } = new();
        public ObservableCollection<DetallePedidoReporte> ReporteDetalle { get; } = new();

        public Pedido Actual
        {
            get => _actual;
            set => SetProperty(ref _actual, value);
        }

        public Pedido? Seleccionado
        {
            get => _seleccionado;
            set
            {
                if (SetProperty(ref _seleccionado, value) && value is not null)
                {
                    Actual = new Pedido
                    {
                        PedidoID = value.PedidoID,
                        ClienteID = value.ClienteID,
                        EmpleadoID = value.EmpleadoID,
                        FechaPedido = value.FechaPedido,
                        FechaRequerida = value.FechaRequerida,
                        FechaEnvio = value.FechaEnvio,
                        TransportistaID = value.TransportistaID,
                        Destinatario = value.Destinatario,
                        CiudadDestino = value.CiudadDestino,
                        PaisDestino = value.PaisDestino
                    };
                }
            }
        }

        public DateTime FechaInicioReporte { get => _fechaInicioReporte; set => SetProperty(ref _fechaInicioReporte, value); }
        public DateTime FechaFinReporte { get => _fechaFinReporte; set => SetProperty(ref _fechaFinReporte, value); }
        public decimal TotalReporte { get => _totalReporte; private set => SetProperty(ref _totalReporte, value); }

        public AsyncRelayCommand CargarCommand { get; }
        public RelayCommand NuevoCommand { get; }
        public AsyncRelayCommand GuardarCommand { get; }
        public AsyncRelayCommand EliminarCommand { get; }
        public AsyncRelayCommand GenerarReporteCommand { get; }

        public PedidoViewModel()
        {
            CargarCommand = new AsyncRelayCommand(_ => CargarAsync());
            NuevoCommand = new RelayCommand(_ => Nuevo());
            GuardarCommand = new AsyncRelayCommand(_ => GuardarAsync());
            EliminarCommand = new AsyncRelayCommand(_ => EliminarAsync(), _ => Seleccionado is not null);
            GenerarReporteCommand = new AsyncRelayCommand(_ => GenerarReporteAsync());
        }

        public async Task CargarAsync()
        {
            try
            {
                var datos = await _repo.ListarTodosAsync();
                Pedidos.Clear();
                foreach (var p in datos)
                    Pedidos.Add(p);
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
            Actual = new Pedido();
        }

        private async Task GuardarAsync()
        {
            try
            {
                if (Actual.PedidoID == 0)
                    await _repo.InsertarAsync(Actual);
                else
                    await _repo.ActualizarAsync(Actual);

                await CargarAsync();
                Nuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar el pedido: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task EliminarAsync()
        {
            if (Seleccionado is null) return;

            var confirmar = MessageBox.Show($"¿Eliminar el pedido #{Seleccionado.PedidoID}?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmar != MessageBoxResult.Yes) return;

            try
            {
                await _repo.EliminarAsync(Seleccionado.PedidoID);
                await CargarAsync();
                Nuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo eliminar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task GenerarReporteAsync()
        {
            try
            {
                if (FechaInicioReporte > FechaFinReporte)
                {
                    MessageBox.Show("La fecha de inicio no puede ser mayor a la fecha fin.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var resultados = await _repo.ListarDetallePorRangoFechasAsync(FechaInicioReporte, FechaFinReporte);

                ReporteDetalle.Clear();
                foreach (var fila in resultados)
                    ReporteDetalle.Add(fila);

                TotalReporte = resultados.Sum(r => r.SubTotal);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo generar el reporte: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}