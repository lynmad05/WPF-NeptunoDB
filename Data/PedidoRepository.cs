using Microsoft.Data.SqlClient;
using System.Data;
using WPF_SP.Models;

namespace WPF_SP.Data
{
    public class PedidoRepository
    {
        public List<Pedido> ListarTodos()
        {
            var lista = new List<Pedido>();
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Pedidos_Listar", conn) { CommandType = CommandType.StoredProcedure };

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add(MapearPedido(reader));

            return lista;
        }

        public Pedido? ObtenerPorId(int pedidoId)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Pedidos_ObtenerPorId", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdPedido", pedidoId);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapearPedido(reader) : null;
        }

        public int Insertar(Pedido p)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Pedidos_Insertar", conn) { CommandType = CommandType.StoredProcedure };

            AgregarParametrosComunes(cmd, p);
            var outId = new SqlParameter("@IdPedido", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outId);

            conn.Open();
            cmd.ExecuteNonQuery();
            return (int)outId.Value;
        }

        public void Actualizar(Pedido p)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Pedidos_Actualizar", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdPedido", p.PedidoID);
            AgregarParametrosComunes(cmd, p);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int pedidoId)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Pedidos_Eliminar", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdPedido", pedidoId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public List<DetallePedidoReporte> ListarDetallePorRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = new List<DetallePedidoReporte>();
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_DetallesPedidos_ReportePorFechas", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
            cmd.Parameters.AddWithValue("@FechaFin", fechaFin.Date);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new DetallePedidoReporte
                {
                    PedidoID = (int)reader["IdPedido"],
                    FechaPedido = (DateTime)reader["FechaPedido"],
                    NombreCliente = reader["NombreCliente"] as string,
                    NombreProducto = reader["NombreProducto"].ToString()!,
                    PrecioUnidad = (decimal)reader["PrecioUnidad"],
                    Cantidad = (short)reader["Cantidad"],
                    Descuento = (decimal)reader["Descuento"],
                    SubTotal = (decimal)reader["Subtotal"]
                });
            }

            return lista;
        }

        private static void AgregarParametrosComunes(SqlCommand cmd, Pedido p)
        {
            cmd.Parameters.AddWithValue("@IdCliente", (object?)p.ClienteID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdEmpleado", (object?)p.EmpleadoID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaPedido", p.FechaPedido);
            cmd.Parameters.AddWithValue("@FechaRequerida", (object?)p.FechaRequerida ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaEnvio", (object?)p.FechaEnvio ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdTransportista", (object?)p.TransportistaID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NombreDestinatario", (object?)p.Destinatario ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CiudadDestino", (object?)p.CiudadDestino ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PaisDestino", (object?)p.PaisDestino ?? DBNull.Value);
        }

        private static Pedido MapearPedido(SqlDataReader reader) => new()
        {
            PedidoID = (int)reader["IdPedido"],
            ClienteID = reader["IdCliente"] as int?,
            EmpleadoID = reader["IdEmpleado"] as int?,
            FechaPedido = (DateTime)reader["FechaPedido"],
            FechaRequerida = reader["FechaRequerida"] as DateTime?,
            FechaEnvio = reader["FechaEnvio"] as DateTime?,
            TransportistaID = reader["IdTransportista"] as int?,
            Destinatario = reader["NombreDestinatario"] as string,
            CiudadDestino = reader["CiudadDestino"] as string,
            PaisDestino = reader["PaisDestino"] as string
        };
    }
}