namespace WPF_SP.Models
{
    public class Pedido : ModelBase
    {
        private int _pedidoID;
        private int? _clienteID;
        private int? _empleadoID;
        private DateTime _fechaPedido = DateTime.Today;
        private DateTime? _fechaRequerida;
        private DateTime? _fechaEnvio;
        private int? _transportistaID;
        private string? _destinatario;
        private string? _ciudadDestino;
        private string? _paisDestino;

        public int PedidoID { get => _pedidoID; set => SetProperty(ref _pedidoID, value); }
        public int? ClienteID { get => _clienteID; set => SetProperty(ref _clienteID, value); }
        public int? EmpleadoID { get => _empleadoID; set => SetProperty(ref _empleadoID, value); }
        public DateTime FechaPedido { get => _fechaPedido; set => SetProperty(ref _fechaPedido, value); }
        public DateTime? FechaRequerida { get => _fechaRequerida; set => SetProperty(ref _fechaRequerida, value); }
        public DateTime? FechaEnvio { get => _fechaEnvio; set => SetProperty(ref _fechaEnvio, value); }
        public int? TransportistaID { get => _transportistaID; set => SetProperty(ref _transportistaID, value); }
        public string? Destinatario { get => _destinatario; set => SetProperty(ref _destinatario, value); }
        public string? CiudadDestino { get => _ciudadDestino; set => SetProperty(ref _ciudadDestino, value); }
        public string? PaisDestino { get => _paisDestino; set => SetProperty(ref _paisDestino, value); }
    }
}
