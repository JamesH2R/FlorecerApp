using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FluorecerApp_Client.Entities
{
    public class AppointmentsEnt
    {
        public long AppointmentId { get; set; }

        public DateTime DateTime { get; set; }

        public long AppointmentType { get; set; }

        public long UserId { get; set; }

        public bool Status { get; set; }
    }
}