using FluorecerApp_Client.Entities;
using FluorecerApp_Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FluorecerApp_Client.Controllers
{
    public class AppointmentController : Controller
    {

        private readonly string apiBaseUrl = "https://localhost:44342/";
        // GET: Appointment
        public async Task<ActionResult> Index()
        {
            if (Session["UserId"] != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiBaseUrl + "api/Appointments");

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var appointments = JsonConvert.DeserializeObject<List<AppointmentsEnt>>(content);
                        return View(appointments);
                    }
                    else
                    {
                        return View(new List<AppointmentsEnt>());
                    }
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Inicia sesión para reservar tu cita";
                return RedirectToAction("Login", "User", TempData);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SetAppointment(int appointmentId)
        {
            try
            {
                //var userId = ObtenerIdDeUsuarioActual();

                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = apiBaseUrl + $"api/SetAppointment/{appointmentId}";

                    HttpResponseMessage response = await client.PostAsync(apiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Cita reservada exitosamente.";
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error al reservar la cita. Por favor, inténtalo de nuevo.";
                        return RedirectToAction("Index", "User");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al reservar la cita: " + ex.Message;
                return RedirectToAction("Index", "User");
            }
        }




    }
}