// Modelo `Usuario` sencillo: propiedades que reflejan la tabla `Registro`.
namespace TP05.Models
{
	public class Usuario
	{
		public int Id { get; set; }
		public string NombreUsuario { get; set; }
		public string Contrasena { get; set; }
		public string Nombre { get; set; }
		public string Apellido { get; set; }
		public string TipoUsuario { get; set; }
	}
}

