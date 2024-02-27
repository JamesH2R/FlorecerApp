using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FluorecerApp_Client.Entities
{
    public class UsersEnt
    {
        public long UserId { get; set; }

        [Required(ErrorMessage = "El campo nombre es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo apellido es obligatorio.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El campo correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Por favor, ingresa una dirección de correo electrónico válida.")]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).*$", ErrorMessage = "La contraseña debe contener al menos 1 mayúscula, 1 número y 1 carácter especial.")]
        [MinLength(8, ErrorMessage = "La contraseña debe contener al menos 8 caracteres.")]
        [Required(ErrorMessage = "El campo contraseña es obligatorio.")]
        public string Password { get; set; }

        [RegularExpression("^[0-9]{8}$", ErrorMessage = "El número de telefono debe ser de 8 números")]
        [Required(ErrorMessage = "El campo celular es obligatorio.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El campo dirección es obligatorio.")]
        public string Address { get; set; }

        public long RoleId { get; set; }

        public string RoleName { get; set; }

        public string Token { get; set; }

        public bool Status { get; set; }

        public string NewPassword { get; set; }


        public string ConfirmNewPassword { get; set; }
    }
}