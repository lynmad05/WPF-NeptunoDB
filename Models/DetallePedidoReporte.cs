namespace WPF_SP.Models
{
    /// <summary>
    /// Fila de resultado para el reporte de DetallePedidos + INNER JOIN Pedidos
    /// (sp_DetallesPedidos_ReportePorFechas). Excluye pedidos con Activo = 0.
    /// </summary>
    public class DetallePedidoReporte
    {
        public int PedidoID { get; set; }
        public DateTime FechaPedido { get; set; }
        public string? NombreCliente { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public decimal PrecioUnidad { get; set; }
        public short Cantidad { get; set; }
        public decimal Descuento { get; set; }
        public decimal SubTotal { get; set; }
    }
}