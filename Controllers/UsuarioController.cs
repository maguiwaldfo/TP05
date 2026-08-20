using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TP05.Models;

namespace TP05.Controllers;

public class UsuarioController : Controller
{
    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registro(Usuario usuario)
    {
        BD bd = new BD();

        if (usuario.NombreUsuario == "" ||
            usuario.Contraseña == "" ||
            usuario.Nombre == "" ||
            usuario.Apellido == "" ||
            usuario.TipoUsuario == "")
        {
            ViewBag.Error = "Complete todos los campos";
            return View(usuario);
        }

        if (bd.UsernameExists(usuario.NombreUsuario))
        {
            ViewBag.Error = "El nombre de usuario ya existe";
            return View(usuario);
        }

        bool registrado = bd.RegistrarUsuario(usuario);

        if (registrado)
        {
            return RedirectToAction("Login");
        }

        ViewBag.Error = "No se pudo registrar el usuario";

        return View(usuario);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string NombreUsuario, string Contraseña)
    {
        BD bd = new BD();

        if (NombreUsuario == "" || Contraseña == "")
        {
            ViewBag.Error = "Complete todos los campos";
            return View();
        }

        Usuario usuario = bd.ValidarCredenciales(
            NombreUsuario,
            Contraseña
        );

        if (usuario != null)
        {
            HttpContext.Session.SetInt32("Id", usuario.Id);
            HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);
            HttpContext.Session.SetString("Nombre", usuario.Nombre);
            HttpContext.Session.SetString("Apellido", usuario.Apellido);
            HttpContext.Session.SetString("TipoUsuario", usuario.TipoUsuario);

            return RedirectToAction("Bienvenida");
        }

        ViewBag.Error = "Usuario o contraseña incorrectos";

        return View();
    }

    public IActionResult Bienvenida()
    {
        string nombreUsuario =
            HttpContext.Session.GetString("NombreUsuario");

        if (string.IsNullOrEmpty(nombreUsuario))
        {
            return RedirectToAction("Login");
        }

        BD bd = new BD();

        List<Usuario> usuarios = bd.ObtenerUsuarios();

        Usuario usuario = usuarios.FirstOrDefault(
            u => u.NombreUsuario == nombreUsuario
        );

        return View(usuario);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Login");
    }
}