using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;

namespace TP05.Models
{
    public class BD
    {
        private string _connectionString = @"Server=localhost;DataBase=RegistroUsuario;Integrated Security=True;TrustServerCertificate=True;";

        public List<Usuario> ObtenerUsuarios()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Registro";

                return connection.Query<Usuario>(query).ToList();
            }
        }

        public bool UsernameExists(string nombreUsuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(1) FROM Registro WHERE nombreUsuario = @NombreUsuario";

                int cantidad = connection.ExecuteScalar<int>(
                    query,
                    new
                    {
                        NombreUsuario = nombreUsuario
                    }
                );

                return cantidad > 0;
            }
        }

        public bool RegistrarUsuario(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Registro (nombreUsuario, contraseña, nombre, apellido, tipoUsuario) VALUES (@NombreUsuario, @Contraseña, @Nombre, @Apellido, @TipoUsuario)";

                int cantidad = connection.Execute(query, usuario);

                return cantidad > 0;
            }
        }

        public Usuario ValidarCredenciales(string nombreUsuario, string contraseña)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Registro WHERE nombreUsuario = @NombreUsuario AND contraseña = @Contraseña";

                List<Usuario> usuarios = connection.Query<Usuario>(
                    query,
                    new
                    {
                        NombreUsuario = nombreUsuario,
                        Contraseña = contraseña
                    }
                ).ToList();

                if (usuarios.Count > 0)
                {
                    return usuarios[0];
                }

                return null;
            }
        }
    }
}