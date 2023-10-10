using FluorecerApp_Client.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Http.Json;

namespace FluorecerApp_Client.Models
{
    public class UsersModel
    {
        public UsersEnt ConsultUsers(long q)
        {
            using (var client = new HttpClient())
            {


                string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultUsers?q=" + q;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<UsersEnt>().Result;
                }

                return null;
            }
        }

        public List<UsersEnt> ConsultUsers()
        {
            using (var client = new HttpClient())
            {


                string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultUsers";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<List<UsersEnt>>().Result;
                }

                return new List<UsersEnt>();
            }
        }


        public UsersEnt ConsultUser(long q)
        {
            using (var client = new HttpClient())
            {


                string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultUser?q=" + q;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<UsersEnt>().Result;
                }

                return null;
            }
        }


        public List<RolesEnt> ConsultRoles()
        {
            using (var client = new HttpClient())
            {

                string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultRoles";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<List<RolesEnt>>().Result;
                }

                return new List<RolesEnt>();
            }
        }

        public int ChangeData(UsersEnt entidad)
        {
            using (var client = new HttpClient())
            {

                string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ChangeData";
                JsonContent body = JsonContent.Create(entidad); //Serializar

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.PutAsync(url, body).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<int>().Result;
                }

                return 0;


            }
        }


        public int EditUser(UsersEnt entidad)
        {
            using (var client = new HttpClient())
            {

                string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/EditUser";
                JsonContent body = JsonContent.Create(entidad); //Serializar

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.PutAsync(url, body).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<int>().Result;
                }

                return 0;
            }
        }


    }
}