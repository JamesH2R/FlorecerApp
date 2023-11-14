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

        public DateTime Date { get; set; }

        public string Hour { get; set; }

        public bool Available { get; set; }

        public long? UserId { get; set; } 
    }
}