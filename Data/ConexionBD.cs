using Microsoft.Data.SqlClient;
using System.Configuration;

namespace WPF_SP.Data
{
    public static class ConexionBD
    {
        public static SqlConnection ObtenerConexion()
        {
            var cadena = ConfigurationManager.ConnectionStrings["NeptunoDB"].ConnectionString;
            return new SqlConnection(cadena);
        }
    }
}
