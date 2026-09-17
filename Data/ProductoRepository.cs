using Microsoft.Data.SqlClient;
using System.Data;
using WPF_SP.Models;

namespace WPF_SP.Data
{
    public class ProductoRepository
    {
        public List<Producto> ListarTodos()
        {
            var lista = new List<Producto>();
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Productos_Listar", conn) { CommandType = CommandType.StoredProcedure };

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add(MapearProducto(reader));

            return lista;
        }

        public Producto? ObtenerPorId(int productoId)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Productos_ObtenerPorId", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdProducto", productoId);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapearProducto(reader) : null;
        }

        public int Insertar(Producto p)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Productos_Insertar", conn) { CommandType = CommandType.StoredProcedure };

            AgregarParametrosComunes(cmd, p);
            var outId = new SqlParameter("@IdProducto", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outId);

            conn.Open();
            cmd.ExecuteNonQuery();
            return (int)outId.Value;
        }

        public void Actualizar(Producto p)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Productos_Actualizar", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdProducto", p.ProductoID);
            AgregarParametrosComunes(cmd, p);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

 
        public void Eliminar(int productoId)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Productos_Eliminar", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdProducto", productoId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        private static void AgregarParametrosComunes(SqlCommand cmd, Producto p)
        {
            cmd.Parameters.AddWithValue("@NombreProducto", p.NombreProducto);
            cmd.Parameters.AddWithValue("@IdProveedor", (object?)p.ProveedorID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdCategoria", (object?)p.CategoriaID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CantidadPorUnidad", (object?)p.CantidadPorUnidad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PrecioUnidad", p.PrecioUnidad);
            cmd.Parameters.AddWithValue("@UnidadesEnExistencia", p.UnidadesEnExistencia);
            cmd.Parameters.AddWithValue("@UnidadesEnPedido", p.UnidadesEnPedido);
            cmd.Parameters.AddWithValue("@NivelReorden", p.NivelDeReorden);
            cmd.Parameters.AddWithValue("@Descontinuado", p.Descontinuado);
        }

        private static Producto MapearProducto(SqlDataReader reader) => new()
        {
            ProductoID = (int)reader["IdProducto"],
            NombreProducto = reader["NombreProducto"].ToString()!,
            ProveedorID = reader["IdProveedor"] as int?,
            CategoriaID = reader["IdCategoria"] as int?,
            CantidadPorUnidad = reader["CantidadPorUnidad"] as string,
            PrecioUnidad = (decimal)reader["PrecioUnidad"],
            UnidadesEnExistencia = (short)reader["UnidadesEnExistencia"],
            UnidadesEnPedido = (short)reader["UnidadesEnPedido"],
            NivelDeReorden = (short)reader["NivelReorden"],
            Descontinuado = (bool)reader["Descontinuado"]
        };
    }
}