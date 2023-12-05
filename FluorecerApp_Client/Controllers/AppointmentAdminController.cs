using FluorecerApp_Client.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FluorecerApp_Client.Controllers
{
    public class AppointmentAdminController : Controller
    {

        private readonly string apiBaseUrl = "https://localhost:44342/";
        // GET: AppointmentAdmin
        public async Task<ActionResult> AppointmentList()
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
        public async Task<ActionResult> CreateAppointment(AppointmentsEnt newAppointment)
        {
            try
            {
                var userId = Session["UserId"];

                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = apiBaseUrl + "api/CreateAppointment";

                    string jsonData = JsonConvert.SerializeObject(newAppointment);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.CitaCreada = true;
                        return RedirectToAction("AppointmentList", "AppointmentAdmin");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error al crear la cita. Por favor, inténtalo de nuevo.";
                        ViewBag.CitaCreada = false;
                        return RedirectToAction("Index", "User");
                    }
                }


            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al crear la cita: " + ex.Message;
                return RedirectToAction("Index", "User");
            }
        }

        public async Task<ActionResult> DeleteAppointment(long appointmentId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = apiBaseUrl + $"api/DeleteAppointment/{appointmentId}";

                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("AppointmentList", "AppointmentAdmin");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error al eliminar la cita.";
                        return RedirectToAction("AppointmentList", "AppointmentAdmin");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar la cita: " + ex.Message;
                return RedirectToAction("AppointmentList", "AppointmentAdmin");
            }
        }
    }
}