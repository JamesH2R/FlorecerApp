using FluorecerApp_Client.Models;
using FluorecerApp_Client.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Reflection;

namespace FluorecerApp_Client.Controllers
{
    public class TestController : Controller
    {
        TestModel model = new TestModel();

        [HttpGet]
        public ActionResult Assign()
        {
            return View("~/Views/Test/Assign.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> AssignEvaluation(MedicalTestsEnt test, HttpPostedFileBase file)
        {
            if (test == null || file == null || file.ContentLength == 0)
            {
                ViewBag.ErrorMessage = "La evaluación o el archivo es nulo. No se pudo asignar.";
                return View("~/Views/Shared/Error.cshtml");
            }

            try
            {
                var result = await model.AssignEvaluation(test, file.InputStream, file.FileName);
                ViewBag.ResultMessage = result;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar asignar la evaluación: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }

            return View("~/Views/Test/Assign.cshtml");
        }


        [HttpGet]
        public ActionResult TestUsers()
        {
            return View("~/Views/Test/TestUsers.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> DownloadEvaluation()
        {
            try
            {
                // Obtener el UserId de la sesión
                long userId = (long)Session["UserId"];

                var result = await model.DownloadEvaluation(userId);
                ViewBag.ResultMessage = result;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar descargar la evaluación: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }

            return View("~/Views/Test/TestUsers.cshtml");
        }


        [HttpGet]
        public ActionResult SendEvaluation()
        {
            return View("~/Views/Test/SendTest.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> SendResult(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                ViewBag.ErrorMessage = "El archivo es nulo o vacío. No se pudo enviar el resultado.";
                return View("~/Views/Shared/Error.cshtml");
            }

            try
            {
                var result = await model.SendResult(file);
                ViewBag.ResultMessage = result;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar enviar el resultado: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }

            return View("~/Views/Test/SendTest.cshtml");

        }

    }
}