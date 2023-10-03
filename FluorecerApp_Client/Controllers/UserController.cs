using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FluorecerApp_Client.Entities;
using System.Net.Http.Headers;
using FluorecerApp_Client.Models;

namespace FluorecerApp_Client.Controllers
{
    public class UserController : Controller
    {
        private readonly string apiBaseUrl = "https://localhost:44342/";

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            TempData.Remove("ErrorMessage");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoginRequest(LoginEnt entidad)
        {

            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseUrl);

                    string json = JsonConvert.SerializeObject(entidad);
                    HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("api/Login", content);

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        // Reads JSON response
                        string responseContent = await response.Content.ReadAsStringAsync();
                        UsersEnt result = JsonConvert.DeserializeObject<UsersEnt>(responseContent);
                        Session["RoleName"] = result.RoleName;
                        Session["Name"] = result.Name;

                        // Redirects to Index (client or admin) after authentication
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        // Handle error
                        Console.WriteLine("Error al iniciar sesión. Código de estado: " + response?.StatusCode);
                        TempData["ErrorMessage"] = "Credenciales incorrectas. Por favor, inténtalo nuevamente.";
                        return View("Login", entidad);
                    }
                }
            } else
            {
                return View("Login", entidad);
            }
            
        }

        [HttpGet]
        public ActionResult Logout() 
        {
            // Removes the session data
            Session.Clear(); 
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register() 
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegisterRequest(UsersEnt entidad)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiBaseUrl);

                        string json = JsonConvert.SerializeObject(entidad);
                        HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync("api/Register", content);

                        if (response.IsSuccessStatusCode)
                        {
                            // Registration successful
                            TempData["SuccessMessage"] = "Registro exitoso.";
                            return RedirectToAction("Login", entidad);
                        }
                        else
                        {
                            var errorMessageJson = await response.Content.ReadAsStringAsync();
                            var errorMessageObject = JsonConvert.DeserializeObject<ErrorMessageViewModel>(errorMessageJson);
                            var errorMessage = errorMessageObject.Message;

                            if (errorMessage.Equals("El correo ya está registrado.", StringComparison.OrdinalIgnoreCase))
                            {
                                // Email already registered
                                TempData["ErrorMessage"] = "El correo ya está registrado.";
                            }
                            else
                            {
                                // Other response from server
                                TempData["ErrorMessage"] = "Error al registrar el usuario.";
                            }
                            return View("Register", entidad);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error al registrar el usuario: " + ex.Message;
                    return View(entidad);
                }
            } else
            {
                return View("Register", entidad);
            }
            
        }

        [HttpGet]
        public ActionResult PasswordRecovery()
        {
            return View();
        }

    }
}