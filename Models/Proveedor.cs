namespace WPF_SP.Models
{
    public class Proveedor : ModelBase
    {
        private int _proveedorID;
        private string _companiaNombre = string.Empty;
        private string? _nombreContacto;
        private string? _cargoContacto;
        private string? _direccion;
        private string? _ciudad;
        private string? _codigoPostal;
        private string? _pais;
        private string? _telefono;
        private string? _fax;

        public int ProveedorID { get => _proveedorID; set => SetProperty(ref _proveedorID, value); }
        public string CompaniaNombre { get => _companiaNombre; set => SetProperty(ref _companiaNombre, value); }
        public string? NombreContacto { get => _nombreContacto; set => SetProperty(ref _nombreContacto, value); }
        public string? CargoContacto { get => _cargoContacto; set => SetProperty(ref _cargoContacto, value); }
        public string? Direccion { get => _direccion; set => SetProperty(ref _direccion, value); }
        public string? Ciudad { get => _ciudad; set => SetProperty(ref _ciudad, value); }
        public string? CodigoPostal { get => _codigoPostal; set => SetProperty(ref _codigoPostal, value); }
        public string? Pais { get => _pais; set => SetProperty(ref _pais, value); }
        public string? Telefono { get => _telefono; set => SetProperty(ref _telefono, value); }
        public string? Fax { get => _fax; set => SetProperty(ref _fax, value); }
    }
}
