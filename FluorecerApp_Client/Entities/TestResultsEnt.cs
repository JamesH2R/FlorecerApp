using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FluorecerApp_Client.Entities
{
    public class TestResultsEnt
    {
        public long ResultId { get; set; }
        public long RoleId { get; set; }

        [Required(ErrorMessage = "Archivo obligatorio.")]
        public string FilePath { get; set; }
        public DateTime Date { get; set; }
    }

}