using FluorecerApp_Client.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace FluorecerApp_Client.Models
{
    public class AppointmentsModel
    {
        public List<SelectListItem> ConsultAppointTypes()
        {
            using (var client = new HttpClient())
            {


                string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultAppointTypes";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<List<SelectListItem>>().Result;
                }

                return new List<SelectListItem>();
            }
        }
        public List<DateTime> ConsultReservedAppointments()
        {
            using (var client = new HttpClient())
            {
                string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultReservedAppointments";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    // Deserializar el JSON en una lista de DateTime
                    return resp.Content.ReadFromJsonAsync<List<DateTime>>().Result;
                }

                return new List<DateTime>();
            }
        }

        public List<DateTime> ConsultarFechasReservadas()
        {
            using (var client = new HttpClient())
            {
                string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultReservedAppointments";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.GetAsync(url).Result;


                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<List<DateTime>>().Result;
                }
                else
                {
                    // Manejo de error si la solicitud no es exitosa
                    // Puedes manejar el error de la manera que consideres apropiada
                    // Aquí, se está devolviendo null en caso de error, pero puedes manejarlo de otra forma según tu lógica de la aplicación.
                    return null;
                }

            }
        }

        public long RegisterAppointment(AppointmentsEnt entidad)
        {
            using (var client = new HttpClient())
            {
                string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/RegisterAppointment";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Convertir la entidad a contenido JSON
                string jsonContent = JsonConvert.SerializeObject(entidad);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage resp = client.PostAsync(url, content).Result;

                if (resp.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta como cadena JSON
                    string responseContent = resp.Content.ReadAsStringAsync().Result;

                    // Deserializar la cadena JSON para obtener el ID de la cita registrada
                    long appointmentId = JsonConvert.DeserializeObject<long>(responseContent);
                    return appointmentId;
                }
                else
                {
                    // Manejo de error si la solicitud no es exitosa
                    // Puedes manejar el error de la manera que consideres apropiada
                    // Aquí, se está devolviendo un valor predeterminado (-1) en caso de error,
                    // pero puedes manejarlo de otra forma según tu lógica de la aplicación.
                    return -1;
                }
            }
        }

        public List<AppointmentsEnt> AdminAppointments(int? pageNumber, int? pageSize)
        {
            using (var client = new HttpClient())
            {
                //string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/AdminAppointments";

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.GetAsync($"{url}?pageNumber={pageNumber}&pageSize={pageSize}").Result;

                if (resp.IsSuccessStatusCode)
                {
                    string jsonContent = resp.Content.ReadAsStringAsync().Result;
                    List<AppointmentsEnt> appointments = JsonConvert.DeserializeObject<List<AppointmentsEnt>>(jsonContent);
                    return appointments;
                }

                return new List<AppointmentsEnt>();
            }
        }

        public List<AppointmentsEnt> UsersAppointments(long userId)
        {
            using (var client = new HttpClient())
            {
                //string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + $"api/UsersAppointments/{userId}";

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    string jsonContent = resp.Content.ReadAsStringAsync().Result;
                    List<AppointmentsEnt> appointments = JsonConvert.DeserializeObject<List<AppointmentsEnt>>(jsonContent);
                    return appointments;
                }

                return new List<AppointmentsEnt>();
            }
        }


        public async Task<string> CancelAppointment(int AppointmentId)
        {
            using (var client = new HttpClient())
            {
                //string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + $"api/CancelAppointment/{AppointmentId}";

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return "Cita cancelada exitosamente";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return "Cita no encontrada";
                }
                else
                {
                    return "Error al cancelar la cita";
                }
            }
        }


    }
}
