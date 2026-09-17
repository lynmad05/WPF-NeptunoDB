using Microsoft.Data.SqlClient;
using System.Data;
using WPF_SP.Models;

namespace WPF_SP.Data
{
    public class ProveedorRepository
    {
        public List<Proveedor> ListarTodos()
        {
            var lista = new List<Proveedor>();
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Proveedores_Listar", conn) { CommandType = CommandType.StoredProcedure };

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add(MapearProveedor(reader));

            return lista;
        }

        public List<Proveedor> BuscarPorContactoYCiudad(string? nombreContacto, string? ciudad)
        {
            var lista = new List<Proveedor>();
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Proveedores_BuscarPorContactoCiudad", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@NombreContacto", string.IsNullOrWhiteSpace(nombreContacto) ? DBNull.Value : nombreContacto);
            cmd.Parameters.AddWithValue("@Ciudad", string.IsNullOrWhiteSpace(ciudad) ? DBNull.Value : ciudad);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add(MapearProveedor(reader));

            return lista;
        }

        public Proveedor? ObtenerPorId(int proveedorId)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Proveedores_ObtenerPorId", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdProveedor", proveedorId);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapearProveedor(reader) : null;
        }

        public int Insertar(Proveedor p)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Proveedores_Insertar", conn) { CommandType = CommandType.StoredProcedure };

            AgregarParametrosComunes(cmd, p);
            var outId = new SqlParameter("@IdProveedor", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outId);

            conn.Open();
            cmd.ExecuteNonQuery();
            return (int)outId.Value;
        }

        public void Actualizar(Proveedor p)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Proveedores_Actualizar", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdProveedor", p.ProveedorID);
            AgregarParametrosComunes(cmd, p);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Baja lógica: el SP hace UPDATE Activo = 0, nunca DELETE físico.
        /// </summary>
        public void Eliminar(int proveedorId)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Proveedores_Eliminar", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdProveedor", proveedorId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        private static void AgregarParametrosComunes(SqlCommand cmd, Proveedor p)
        {
            cmd.Parameters.AddWithValue("@NombreCompania", p.CompaniaNombre);
            cmd.Parameters.AddWithValue("@NombreContacto", (object?)p.NombreContacto ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Cargo", (object?)p.CargoContacto ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Direccion", (object?)p.Direccion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Ciudad", (object?)p.Ciudad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CodigoPostal", (object?)p.CodigoPostal ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Pais", (object?)p.Pais ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefono", (object?)p.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Fax", (object?)p.Fax ?? DBNull.Value);
        }

        private static Proveedor MapearProveedor(SqlDataReader reader) => new()
        {
            ProveedorID = (int)reader["IdProveedor"],
            CompaniaNombre = reader["NombreCompania"].ToString()!,
            NombreContacto = reader["NombreContacto"] as string,
            CargoContacto = reader["Cargo"] as string,
            Direccion = reader["Direccion"] as string,
            Ciudad = reader["Ciudad"] as string,
            CodigoPostal = reader["CodigoPostal"] as string,
            Pais = reader["Pais"] as string,
            Telefono = reader["Telefono"] as string,
            Fax = reader["Fax"] as string
        };
    }
}