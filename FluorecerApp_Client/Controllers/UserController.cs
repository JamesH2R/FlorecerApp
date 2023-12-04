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
using System.Net.Sockets;
using System.Net;

namespace FluorecerApp_Client.Controllers
{
    public class UserController : Controller
    {

        UsersModel model = new UsersModel();


        private readonly string apiBaseUrl = "https://localhost:44342/";

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                long userId = (long)Session["UserId"];

                string lastname = Session["Lastname"] as string;
                string email = Session["Email"] as string;
                string name = Session["Name"] as string;
                string phone = Session["Phone"] as string;
                string address = Session["Address"] as string;

                // Crea un objeto para representar al usuario
                var user = new UsersEnt
                {
                    UserId = userId,
                    Name = name,
                    LastName = lastname,
                    Email = email,
                    Phone = phone,
                    Address = address,
                };

                return View(user);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
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
                        Session["UserId"] = result.UserId;
                        Session["Email"] = result.Email;
                        Session["NameLastName"] = result.LastName;
                        Session["Token"] = result.Token;
                        Session["RoleName"] = result.RoleName;
                        Session["Name"] = result.Name;
                        Session["Email"] = result.Email;
                        Session["UserId"] = result.UserId;
                        Session["Phone"] = result.Phone;
                        Session["Address"] = result.Address;
                        

                        // Redirects to Index (client or admin) after authentication
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            TempData["ErrorMessage"] = "El usuario está inactivo. Por favor, contacta al administrador.";
                        } else
                        {
                            Console.WriteLine("Error al iniciar sesión. Código de estado: " + response?.StatusCode);
                            TempData["ErrorMessage"] = "Credenciales incorrectas. Por favor, inténtalo nuevamente.";
                        }
                        
                        return View("Login", entidad);
                    }
                }
            }
            else
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
            if (ModelState.IsValid)
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
                            TempData["SuccessMessage"] = "Registro exitoso. Proceda a iniciar sesión";
                            return RedirectToAction("Login");
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
            }
            else
            {
                return View("Register", entidad);
            }

        }

        [HttpGet]
        public ActionResult PasswordRecovery()
        {
            return View();
        }


        [HttpGet]
        public ActionResult ConsultUsers()
        {
            var resp = model.ConsultUsers();
            return View();
        }



        [HttpGet]
        public ActionResult Edit()
        {
            var resp = model.ConsultUser(long.Parse(Session["UserId"].ToString()));
            var respRoles = model.ConsultRoles();

            var roles = new List<SelectListItem>();
            foreach (var item in respRoles)
            {
                roles.Add(new SelectListItem { Value = item.RoleId.ToString(), Text = item.RoleName.ToString() });
            }

            ViewBag.ComboRoles = roles;
            return View(resp);
        }

        [HttpPost]
        public async Task<ActionResult> EditUser(UsersEnt entidad)
        {
            // Obtén el UserId de la sesión
            long userId = (long)Session["UserId"];


            entidad.UserId = userId;

            var resp = model.EditUser(entidad);

            if (resp > 0)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                ViewBag.MsjPantalla = "No se ha podido actualizar la información del usuario";
                return View("Profile");
            }
        }

        [HttpGet]
        public  ActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public new ActionResult EditProfile()
        {
            return View();
        }



    }
}

