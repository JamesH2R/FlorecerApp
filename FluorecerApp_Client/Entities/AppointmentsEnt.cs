using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FluorecerApp_Client.Entities
{
    public class AppointmentsEnt
    {
        public long AppoimentId { get; set; }
        public long PatientId { get; set; }

        [Required(ErrorMessage = "El campo fecha es obligatorio.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "El campo hora es obligatorio.")]
        public string Hour { get; set; }
        public string Notes { get; set; }
    }
}