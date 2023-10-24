using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using FluorecerApp_Client.Entities;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;

namespace FluorecerApp_Client.Models
{
    public class UserAdminModel
    {
        public List<UsersEnt> UserConsultation(int? pageNumber, int? pageSize)
        {
            using (var client = new HttpClient())
            {
                //string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/UserConsultation";

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.GetAsync($"{url}?pageNumber={pageNumber}&pageSize={pageSize}").Result;

                if (resp.IsSuccessStatusCode)
                {
                    string jsonContent = resp.Content.ReadAsStringAsync().Result;
                    List<UsersEnt> users = JsonConvert.DeserializeObject<List<UsersEnt>>(jsonContent);
                    return users;
                }

                return new List<UsersEnt>();
            }
        }

        public async Task<string> InactivateUser(int userId)
        {
            using (var client = new HttpClient())
            {
                //string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + $"api/InactivateUser/{userId}";

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = await client.PutAsync(url, null);

                if (resp.IsSuccessStatusCode)
                {
                    return "Usuario inactivado con éxito";
                }

                return "No se pudo inactivar al usuario";
            }
        }

        public List<UsersEnt> SearchUsers(string searchTerm)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + $"api/SearchUsers?searchTerm={searchTerm}";
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    List<UsersEnt> searchResults = JsonConvert.DeserializeObject<List<UsersEnt>>(content);
                    return searchResults;
                }

                return new List<UsersEnt>();
            }
        }

    }
}