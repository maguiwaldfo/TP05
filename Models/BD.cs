// Clase BD: operaciones de base de datos usando Dapper y Microsoft.Data.SqlClient.
// Contiene métodos simples para listar usuarios, verificar existencia,
// registrar y validar credenciales (estilo TP03ahorcado).
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;

namespace TP05.Models
{
    public class BD
    {
        // Cadena de conexión (ajustar según tu entorno)
        private string _connectionString = @"Server=localhost;DataBase=RegistroUsuario;Integrated Security=True;TrustServerCertificate=True;";

        // Devuelve todos los usuarios de la tabla Registro
        public List<Usuario> ObtenerUsuarios()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT id AS Id, nombreUsario AS NombreUsuario, contraseña AS Contrasena, nombre AS Nombre, apellido AS Apellido, tipoUsuario AS TipoUsuario FROM Registro";
                    return connection.Query<Usuario>(query).ToList();
                }
            }
            catch (Exception)
            {
                return new List<Usuario>();
            }
        }

        // Verifica si ya existe un nombre de usuario
        public bool UsernameExists(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT COUNT(1) FROM Registro WHERE nombreUsario = @usuario";
                    int count = connection.ExecuteScalar<int>(query, new { usuario = username });
                    return count > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Inserta un nuevo usuario (calcula id = MAX(id)+1)
        public bool RegistrarUsuario(Usuario u)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sql = @"
DECLARE @newId INT;
SELECT @newId = ISNULL(MAX(id), 0) + 1 FROM Registro;
INSERT INTO Registro (nombreUsario, contraseña, nombre, id, apellido, tipoUsuario)
VALUES (@NombreUsuario, @Contrasena, @Nombre, @newId, @Apellido, @TipoUsuario);
";
                    int rows = connection.Execute(sql, new { u.NombreUsuario, u.Contrasena, u.Nombre, u.Apellido, u.TipoUsuario });
                    return rows > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Valida credenciales; devuelve el usuario si coinciden, o null
        public Usuario ValidarCredenciales(string usuario, string contrasena)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT id AS Id, nombreUsario AS NombreUsuario, contraseña AS Contrasena, nombre AS Nombre, apellido AS Apellido, tipoUsuario AS TipoUsuario FROM Registro WHERE nombreUsario = @usuario AND contraseña = @contrasena";
                    return connection.QueryFirstOrDefault<Usuario>(query, new { usuario, contrasena });
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
