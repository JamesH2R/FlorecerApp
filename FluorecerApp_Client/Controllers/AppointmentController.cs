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
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Appointment");
            }
            else
            {
                TempData["ErrorMessage"] = "Inicia sesión para reservar tu cita";
                return RedirectToAction("Login", "User", TempData);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAppointment(AppointmentsEnt model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {

                        long patientId = (long)Session["UserId"];
                        model.PatientId = patientId;
                        string notas = model.Notes;
                        client.BaseAddress = new Uri(apiBaseUrl);

                        string json = JsonConvert.SerializeObject(model);
                        HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync("api/NewAppointment", content);

                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Reserva exitosa.";
                            return RedirectToAction("Index", "User");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Error reservando su cita";
                            return View("Index", model);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error al reservar la cita: " + ex.Message;
                    return View(model);
                }
            }
            else
            {
                return View("Index", model);
            }
        }

    }
}