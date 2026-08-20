using System;
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
                string query = "SELECT id AS Id, nombreUsario AS NombreUsuario, contraseña AS Contraseña, nombre AS Nombre, apellido AS Apellido, tipoUsuario AS TipoUsuario FROM Registro";

                return connection.Query<Usuario>(query).ToList();
            }
        }

        public bool UsernameExists(string usuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(1) FROM Registro WHERE nombreUsario = @usuario";

                int cantidad = connection.ExecuteScalar<int>(
                    query,
                    new { usuario }
                );

                return cantidad > 0;
            }
        }

        public bool RegistrarUsuario(Usuario u)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    DECLARE @newId INT;

                    SELECT @newId = ISNULL(MAX(id), 0) + 1
                    FROM Registro;

                    INSERT INTO Registro
                    (id, nombreUsario, contraseña, nombre, apellido, tipoUsuario)
                    VALUES
                    (@newId, @NombreUsuario, @Contraseña, @Nombre, @Apellido, @TipoUsuario);
                ";

                int filas = connection.Execute(sql, new
                {
                    u.NombreUsuario,
                    u.Contraseña,
                    u.Nombre,
                    u.Apellido,
                    u.TipoUsuario
                });

                return filas > 0;
            }
        }

        public Usuario ValidarCredenciales(string usuario, string contraseña)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT
                        id AS Id,
                        nombreUsario AS NombreUsuario,
                        contraseña AS Contraseña,
                        nombre AS Nombre,
                        apellido AS Apellido,
                        tipoUsuario AS TipoUsuario
                    FROM Registro
                    WHERE nombreUsario = @usuario
                    AND contraseña = @contraseña";

                return connection.QueryFirstOrDefault<Usuario>(
                    query,
                    new
                    {
                        usuario,
                        contraseña
                    }
                );
            }
        }
    }
}