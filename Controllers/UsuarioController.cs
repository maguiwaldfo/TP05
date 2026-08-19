using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TP05.Models;

namespace TP05.Controllers;

// Controlador `UsuarioController` siguiendo el estilo simple del TP03ahorcado.
public class UsuarioController : Controller
{
    // Instancia simple de la clase BD (manejo de la BD con Dapper)
    private readonly BD _bd = new BD();

    // GET: Mostrar formulario de registro
    public IActionResult Register()
    {
        return View();
    }

    // POST: Procesar registro de usuario
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(Usuario model)
    {
        // Registro simple: intentar insertar y mostrar la vista de login si se pudo.
        bool ok = _bd.RegistrarUsuario(model);
        if (ok)
        {
            return View("Login");
        }

        // Si falla, volver al formulario de registro (sin mensajes adicionales)
        return View(model);
    }

    // GET: Mostrar formulario de login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Procesar login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string NombreUsuario, string Contraseña)
    {
        // Login simple: validar credenciales y mostrar la vista de bienvenida si coinciden.
        Usuario user = _bd.ValidarCredenciales(NombreUsuario, Contraseña);
        if (user != null)
        {
            // Guardar sesión mínima (sin operadores ??)
            HttpContext.Session.SetString("NombreUsuario", user.NombreUsuario);
            HttpContext.Session.SetString("Nombre", user.Nombre);
            HttpContext.Session.SetString("Apellido", user.Apellido);
            HttpContext.Session.SetString("TipoUsuario", user.TipoUsuario);
            HttpContext.Session.SetInt32("Id", user.Id);

            // Mostrar directamente la vista Bienvenida con el usuario
            return View("Bienvenida", user);
        }

        // Credenciales inválidas: volver al formulario de login
        return View();
    }

    // GET: Página privada de bienvenida
    public IActionResult Bienvenida()
    {
        string nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
        if (string.IsNullOrEmpty(nombreUsuario))
        {
            return RedirectToAction("Login");
        }

        List<Usuario> usuarios = _bd.ObtenerUsuarios();
        Usuario usuario = usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario);
        return View(usuario);
    }

    // GET: Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
