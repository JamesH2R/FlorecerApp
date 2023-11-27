using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FluorecerApp_Client.Entities
{
    public class MedicalTestsEnt
    {
        public long TestId { get; set; }
        [Required(ErrorMessage = "Campo obligatorio.")]
        public long UserId { get; set; }
        public string LastName { get; set; }
        public long RoleId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime Date { get; set; }
    }


}