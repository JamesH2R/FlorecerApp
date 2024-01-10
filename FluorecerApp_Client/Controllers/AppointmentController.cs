using FluorecerApp_Client.Entities;
using FluorecerApp_Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PagedList;

namespace FluorecerApp_Client.Controllers
{
    public class AppointmentController : Controller
    {

        UsersModel model = new UsersModel();
        AppointmentsModel modelA = new AppointmentsModel();

        /*

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

       */


        /* 

         [HttpGet]
         public ActionResult GetAppointments()
         {


             var UserId = model.ConsultUser(long.Parse(Session["UserId"].ToString()));

             if (!string.IsNullOrEmpty(UserId))
             {
                 ViewBag.UserId = UserId;

                 var fechasReservadas = modelA.ConsultReservedAppointments();
                 ViewBag.FechasReservadas = fechasReservadas;

                 var tiposCita = modelA.ConsultAppointTypes();
                 ViewBag.ConsultarTipoCita = tiposCita;

                 return View();
             }
             else
             {
                 return RedirectToAction("Error", "Home");
             }

         }

       */




        //[HttpGet]
        //public ActionResult GetAppointments(long? idUsuario)
        //{
        //    if (idUsuario != null)
        //    {

        //        ViewBag.UserId = idUsuario.ToString();

        //        var fechasReservadas = modelA.ConsultReservedAppointments();
        //        ViewBag.FechasReservadas = fechasReservadas;

        //        var tiposCita = modelA.ConsultAppointTypes();
        //        ViewBag.ConsultarTipoCita = tiposCita;

        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Error", "Home");
        //    }
        //}








        [HttpGet]
        public ActionResult GetAppointments()
        {
            // Asumo que model es una instancia de la clase que tiene el método ConsultUser
            var user = model.ConsultUser(long.Parse(Session["UserId"].ToString()));

            if (user != null && !string.IsNullOrEmpty(user.UserId.ToString()))
            {
                ViewBag.UserId = user.UserId.ToString();

                var fechasReservadas = modelA.ConsultReservedAppointments();
                ViewBag.FechasReservadas = fechasReservadas;

                var tiposCita = modelA.ConsultAppointTypes();
                ViewBag.ConsultarTipoCita = tiposCita;

                return View();
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public JsonResult ObtenerFechasReservadas()
        {
            var fechasReservadas = modelA.ConsultarFechasReservadas(); // Obtener fechas reservadas desde la base de datos
            var fechasFormateadas = fechasReservadas.Select(fecha => fecha.ToString("yyyy-MM-ddTHH:mm:ss")); // Formatear fechas según el formato deseado

            return Json(fechasFormateadas, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult RegisterAppointment(AppointmentsEnt entidad)
        {


            ViewBag.ConsultarTipoUsuario = modelA.ConsultAppointTypes();
            ViewBag.ConsultarTipoCita = modelA.ConsultAppointTypes();
            var AppointmentId = modelA.RegisterAppointment(entidad);

            if (AppointmentId > 0)
            {
                ViewBag.ConsultarTipoCita = modelA.ConsultAppointTypes();
                return RedirectToAction("GetAppointments", "Appointment");
            }

            ViewBag.ConsultarTipoCita = modelA.ConsultAppointTypes();
            ViewBag.MensajePantalla = "No se pudo registrar la Cita";
            return View();
        }


        [HttpGet]
        public ActionResult AdminAppointments(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10; // Número de citas por página

            var appointments = modelA.AdminAppointments(pageNumber, pageSize);

            var pagedAppointments = appointments.ToPagedList(pageNumber, pageSize);
            return View("~/Views/Appointment/Admin_Appointments.cshtml", pagedAppointments);
        }

        [HttpGet]
        public ActionResult UsersAppointments()
        {
            // Obtén el userId desde tu sesión
            long userId = (long)Session["UserId"];

            var appointments = modelA.UsersAppointments(userId);

            return View("~/Views/Appointment/Users_Appointments.cshtml", appointments);
        }
        

        [HttpPost]
        public async Task<ActionResult> CancelAppointment(int AppointmentId)
        {
            string result = await modelA.CancelAppointment(AppointmentId);

            if (result == "Cita cancelada exitosamente")
            {
                TempData["SuccessMessage"] = result;
                return RedirectToAction("UsersAppointments");
            }
            else
            {
                TempData["ErrorMessage"] = result;
                return RedirectToAction("UsersAppointments");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AdminCancelAppointment(int AppointmentId)
        {
            string result = await modelA.CancelAppointment(AppointmentId);

            if (result == "Cita cancelada exitosamente")
            {
                TempData["SuccessMessage"] = result;
                return RedirectToAction("AdminAppointments");
            }
            else
            {
                TempData["ErrorMessage"] = result;
                return RedirectToAction("AdminAppointments");
            }
        }

    }
}
    

