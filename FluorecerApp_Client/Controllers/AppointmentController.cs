using FluorecerApp_Client.Entities;
using FluorecerApp_Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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
                var userId = Session["UserId"];

                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = apiBaseUrl + $"api/SetAppointment/{appointmentId}?userId={userId}";

                    HttpResponseMessage response = await client.PostAsync(apiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.ReservaExitosa = true;
                        return RedirectToAction("Index", "Appointment");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error al reservar la cita. Por favor, inténtalo de nuevo.";
                        ViewBag.ReservaExitosa = false;
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

        public async Task<ActionResult> GetAppointments()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var userId = Session["UserId"];
                    string apiUrl = apiBaseUrl + $"api/AppointmentsByUserId/{userId}";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var appointments = JsonConvert.DeserializeObject<List<AppointmentsEnt>>(responseContent);
                        return View(appointments);
                    }
                    else
                    {
                        // Manejar el caso en que la solicitud no fue exitosa
                        TempData["ErrorMessage"] = "Error al obtener las citas por userId.";
                        return RedirectToAction("Index", "User");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al obtener las citas por userId: " + ex.Message;
                return RedirectToAction("Index", "User");
            }
        }
        }




    }
