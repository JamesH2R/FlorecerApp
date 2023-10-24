using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FluorecerApp_Client.Entities
{
    public class RolesEnt
    {

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public bool Status { get; set; }
    }

}