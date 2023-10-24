using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluorecerApp_Client.Controllers
{
    public static class AppointmentsHelper
    {

        public static List<SelectListItem> GetHourOptions()
        {
            var hours = new List<SelectListItem>();
            for (int hour = 8; hour <= 15; hour++)
            {
                var formattedHour = hour.ToString("00:00");
                hours.Add(new SelectListItem { Text = formattedHour, Value = formattedHour });
            }
            return hours;
        }
    }
}