namespace WPF_SP.Models
{
    public class Producto : ModelBase
    {
        private int _productoID;
        private string _nombreProducto = string.Empty;
        private int? _proveedorID;
        private int? _categoriaID;
        private string? _cantidadPorUnidad;
        private decimal _precioUnidad;
        private short _unidadesEnExistencia;
        private short _unidadesEnPedido;
        private short _nivelDeReorden;
        private bool _descontinuado;

        public int ProductoID { get => _productoID; set => SetProperty(ref _productoID, value); }
        public string NombreProducto { get => _nombreProducto; set => SetProperty(ref _nombreProducto, value); }
        public int? ProveedorID { get => _proveedorID; set => SetProperty(ref _proveedorID, value); }
        public int? CategoriaID { get => _categoriaID; set => SetProperty(ref _categoriaID, value); }
        public string? CantidadPorUnidad { get => _cantidadPorUnidad; set => SetProperty(ref _cantidadPorUnidad, value); }
        public decimal PrecioUnidad { get => _precioUnidad; set => SetProperty(ref _precioUnidad, value); }
        public short UnidadesEnExistencia { get => _unidadesEnExistencia; set => SetProperty(ref _unidadesEnExistencia, value); }
        public short UnidadesEnPedido { get => _unidadesEnPedido; set => SetProperty(ref _unidadesEnPedido, value); }
        public short NivelDeReorden { get => _nivelDeReorden; set => SetProperty(ref _nivelDeReorden, value); }
        public bool Descontinuado { get => _descontinuado; set => SetProperty(ref _descontinuado, value); }
    }
}
