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
using PagedList;

namespace FluorecerApp_Client.Controllers
{
    public class UserAdminController : Controller
    {
        UserAdminModel model = new UserAdminModel();

        [HttpGet]
        public ActionResult UserConsultation(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 3;

            var users = model.UserConsultation(pageNumber, pageSize); 

            var pagedUsers = users.ToPagedList(pageNumber, pageSize);
            return View("~/Views/UserAdmin/UserAdmin.cshtml", pagedUsers);
        }

        [HttpPost]
        public async Task<ActionResult> InactivateUser(int userId)
        {
            var result = await model.InactivateUser(userId);
            return Content(result, "text/html");
        }

        [HttpGet]
        public ActionResult SearchUsers(string searchTerm)
        {
            var searchResults = model.SearchUsers(searchTerm);
            return View("~/Views/UserAdmin/SearchUsers.cshtml", searchResults);
        }
    }
}