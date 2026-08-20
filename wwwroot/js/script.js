function validarRegistro() {

    const usuario = document.getElementById("NombreUsuario").value;
    const contraseña = document.getElementById("Contraseña").value;
    const nombre = document.getElementById("Nombre").value;
    const apellido = document.getElementById("Apellido").value;
    const tipoUsuario = document.getElementById("TipoUsuario").value;

    if (usuario == "" ||
        contraseña == "" ||
        nombre == "" ||
        apellido == "" ||
        tipoUsuario == "") {

        alert("Complete todos los campos");
        return false;
    }

    if (usuario.length < 4) {

        alert("El nombre de usuario debe tener al menos 4 caracteres");
        return false;
    }

    if (contraseña.length < 6) {

        alert("La contraseña debe tener al menos 6 caracteres");
        return false;
    }

    const letras = /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/;

    if (!letras.test(nombre)) {

        alert("El nombre solo puede contener letras");
        return false;
    }

    if (!letras.test(apellido)) {

        alert("El apellido solo puede contener letras");
        return false;
    }

    return true;
}


function validarLogin() {

    const usuario = document.getElementById("NombreUsuario").value;
    const contraseña = document.getElementById("Contraseña").value;

    if (usuario == "" || contraseña == "") {

        alert("Complete todos los campos");
        return false;
    }

    return true;
}