using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FluorecerApp_Client.Entities
{
    public class LoginEnt
    {
        [Required(ErrorMessage = "El campo correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Por favor, ingresa una dirección de correo electrónico válida.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo contraseña es obligatorio.")]
        public string Password { get; set; }
    }
}