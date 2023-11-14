using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluorecerApp_Client.Controllers
{
    public class AppointmentAdminController : Controller
    {
        // GET: AppointmentAdmin
        public ActionResult AppointmentList()
        {
            return View();
        }
    }
}