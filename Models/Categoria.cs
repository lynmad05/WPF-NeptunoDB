namespace WPF_SP.Models
{
    public class Categoria : ModelBase
    {
        private int _categoriaID;
        private string _nombreCategoria = string.Empty;
        private string? _descripcion;

        public int CategoriaID { get => _categoriaID; set => SetProperty(ref _categoriaID, value); }
        public string NombreCategoria { get => _nombreCategoria; set => SetProperty(ref _nombreCategoria, value); }
        public string? Descripcion { get => _descripcion; set => SetProperty(ref _descripcion, value); }
    }
}
