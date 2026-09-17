using System.Collections.ObjectModel;
using System.Windows;
using WPF_SP.Data;
using WPF_SP.Models;

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

        public DateTime FechaInicioReporte
        {
            get => _fechaInicioReporte;
            set => SetProperty(ref _fechaInicioReporte, value);
        }

        public DateTime FechaFinReporte
        {
            get => _fechaFinReporte;
            set => SetProperty(ref _fechaFinReporte, value);
        }

        public decimal TotalReporte
        {
            get => _totalReporte;
            private set => SetProperty(ref _totalReporte, value);
        }

        public RelayCommand CargarCommand { get; }
        public RelayCommand NuevoCommand { get; }
        public RelayCommand GuardarCommand { get; }
        public RelayCommand EliminarCommand { get; }
        public RelayCommand GenerarReporteCommand { get; }

        public PedidoViewModel()
        {
            CargarCommand = new RelayCommand(_ => Cargar());
            NuevoCommand = new RelayCommand(_ => Nuevo());
            GuardarCommand = new RelayCommand(_ => Guardar());
            EliminarCommand = new RelayCommand(_ => Eliminar(), _ => Seleccionado is not null);
            GenerarReporteCommand = new RelayCommand(_ => GenerarReporte());

            Cargar();
        }

        private void Cargar()
        {
            Pedidos.Clear();
            foreach (var p in _repo.ListarTodos())
                Pedidos.Add(p);
        }

        private void Nuevo()
        {
            Seleccionado = null;
            Actual = new Pedido();
        }

        private void Guardar()
        {
            try
            {
                if (Actual.PedidoID == 0)
                    _repo.Insertar(Actual);
                else
                    _repo.Actualizar(Actual);

                Cargar();
                Nuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar el pedido: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Eliminar()
        {
            if (Seleccionado is null) return;

            var confirmar = MessageBox.Show($"¿Eliminar el pedido #{Seleccionado.PedidoID}?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmar != MessageBoxResult.Yes) return;

            try
            {
                _repo.Eliminar(Seleccionado.PedidoID);
                Cargar();
                Nuevo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo eliminar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerarReporte()
        {
            try
            {
                if (FechaInicioReporte > FechaFinReporte)
                {
                    MessageBox.Show("La fecha de inicio no puede ser mayor a la fecha fin.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ReporteDetalle.Clear();
                var resultados = _repo.ListarDetallePorRangoFechas(FechaInicioReporte, FechaFinReporte);
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
