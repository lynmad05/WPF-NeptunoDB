using Microsoft.Data.SqlClient;
using System.Data;
using WPF_SP.Models;

namespace WPF_SP.Data
{
    public class CategoriaRepository
    {
        public List<Categoria> ListarTodos()
        {
            var lista = new List<Categoria>();
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Categorias_Listar", conn) { CommandType = CommandType.StoredProcedure };

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add(MapearCategoria(reader));

            return lista;
        }

        public Categoria? ObtenerPorId(int categoriaId)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Categorias_ObtenerPorId", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdCategoria", categoriaId);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapearCategoria(reader) : null;
        }

        public int Insertar(Categoria c)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Categorias_Insertar", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@NombreCategoria", c.NombreCategoria);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)c.Descripcion ?? DBNull.Value);
            var outId = new SqlParameter("@IdCategoria", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outId);

            conn.Open();
            cmd.ExecuteNonQuery();
            return (int)outId.Value;
        }

        public void Actualizar(Categoria c)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Categorias_Actualizar", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdCategoria", c.CategoriaID);
            cmd.Parameters.AddWithValue("@NombreCategoria", c.NombreCategoria);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)c.Descripcion ?? DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }


        public void Eliminar(int categoriaId)
        {
            using var conn = ConexionBD.ObtenerConexion();
            using var cmd = new SqlCommand("dbo.sp_Categorias_Eliminar", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdCategoria", categoriaId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        private static Categoria MapearCategoria(SqlDataReader reader) => new()
        {
            CategoriaID = (int)reader["IdCategoria"],
            NombreCategoria = reader["NombreCategoria"].ToString()!,
            Descripcion = reader["Descripcion"] as string
        };
    }
}