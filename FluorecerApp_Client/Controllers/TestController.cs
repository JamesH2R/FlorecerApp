using FluorecerApp_Client.Models;
using FluorecerApp_Client.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http;

namespace FluorecerApp_Client.Controllers
{
    public class TestController : Controller
    {
        TestModel model = new TestModel();

        //ADMIN

        [HttpGet]
        public async Task<ActionResult> Assign()
        {
            if (TempData.ContainsKey("PostEjecutado") && (bool)TempData["PostEjecutado"])
            {

                var lastNames = await model.GetDropdownLastNames();
                ViewBag.LastNames = new SelectList(lastNames, "UserId", "LastName");

                TempData["DesdePost"] = true;
            }
            else
            {
                var lastNames = await model.GetDropdownLastNames();
                ViewBag.LastNames = new SelectList(lastNames, "UserId", "LastName");
                return View("~/Views/Test/Assign.cshtml");
            }
            if (TempData.ContainsKey("ResultMessage")) { ViewBag.ResultMessage = TempData["ResultMessage"]; }
            TempData.Remove("PostEjecutado"); TempData.Remove("ResultMessage"); // Limpiar el mensaje de TempData



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
                var selectedUserId = test.UserId;

                var selectedUser = await model.GetUserById(selectedUserId);
                if (selectedUser != null)
                {
                    // Asignar el UserId, LastName y Name al objeto MedicalTestsEnt
                    test.UserId = selectedUser.UserId;
                    test.LastName = selectedUser.LastName;
                    test.Name = selectedUser.Name;
                }

                // Utilizar el método AssignEvaluation del modelo
                var result = await model.AssignEvaluation(test, file.InputStream, file.FileName);
                ViewBag.ResultMessage = result;

                // Establecer un indicador en TempData para indicar que se ejecutó un POST antes de este método
                TempData["PostEjecutado"] = true;

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar asignar la evaluación: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }

            ViewBag.ResultMessage = "Evaluación asignada con éxito";
            TempData["ResultMessage"] = ViewBag.ResultMessage; // Guardar en TempData

            // Redireccionar al método GET "Assign" del controlador "Test"
            return RedirectToAction("Assign", "Test");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteEvaluations(MedicalTestsEnt test)
        {
            try
            {
                var selectedUserId = test.UserId;

                var selectedUser = await model.GetUserById(selectedUserId);
                if (selectedUser != null)
                {
                    // Asignar el UserId y LastName al objeto MedicalTestsEnt
                    test.UserId = selectedUser.UserId;
                    test.LastName = selectedUser.LastName;
                }


                var result = await model.DeleteEvaluations(test.UserId);
                ViewBag.ResultMessage = result;

                // Establecer un indicador en TempData para indicar que se ejecutó un POST antes de este método
                TempData["PostEjecutado"] = true;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar eliminar la evaluación: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }

            ViewBag.ResultMessage = "Evaluación eliminada con éxito"; TempData["ResultMessage"] = ViewBag.ResultMessage; // Guardar en TempData
            // Redireccionar al método GET "Assign" del controlador "Test"
            return RedirectToAction("Assign", "Test");
        }

        [HttpGet]
        public ActionResult TestUsersDone()
        {
            ViewBag.ResultMessage = TempData["ResultMessage"] as string;
            var resp = model.TestUsersDone();
            return View(resp);
        }
       
        [HttpGet]
        public async Task<ActionResult> DownloadTestResult(long ResultId)
        {
            try
            {
                // Llamar al método DownloadEvaluation que devuelve el archivo para descargar
                var fileBytes = await model.DownloadTestResult(ResultId);

                if (fileBytes != null)
                {
                    // Obtener el nombre del archivo según el ResultId
                    string fileName = GetFileNameForResultId(ResultId);

                    TempData["SuccessMessage"] = "Evaluación descargada con éxito.";
                    return RedirectToAction("TestUsersDone", "Test");
                }
                else
                {
                    ViewBag.ErrorMessage = "El archivo no se encontró para la descarga.";
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar descargar la evaluación: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        private string GetFileNameForResultId(long resultId)
        {
            return $"{resultId}.zip";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteTestResult(long ResultId)
        {
            try
            {
                // Llamar al método DeleteTestResult que realiza la eliminación en el API
                var resultMessage = await model.DeleteTestResult(ResultId);

                ViewBag.ResultMessage = resultMessage;

                // Puedes agregar un TempData para llevar el mensaje a la acción TestUsersDone
                TempData["ResultMessage"] = resultMessage;

                return RedirectToAction("TestUsersDone", "Test");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error al intentar eliminar las evaluaciones: {ex.Message}";
                return View("~/Views/Shared/Error.cshtml");
            }
        }




        //USUARIOS

        public async Task<ActionResult> TestUsers()
        {
            try
            {
                // Obtener el UserId de la sesión
                long userId = (long)Session["UserId"];

                // Llamar a GetUserEvaluationNames para obtener los nombres de archivo
                var fileNames = await model.GetUserEvaluationNames(userId);
                ViewBag.FileNames = fileNames;

                // Cargar la vista TestUsers.cshtml
                return View("~/Views/Test/TestUsers.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar obtener los nombres de archivo de la evaluación: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }
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

        [HttpGet]
        public async Task<ActionResult> GetUserEvaluationNames()
        {
            try
            {
                // Obtener el UserId de la sesión
                long userId = (long)Session["UserId"];

                var fileNames = await model.GetUserEvaluationNames(userId);
                ViewBag.FileNames = fileNames;

                // Pasar a la vista la lista de nombres de los archivos
                return View("~/Views/Test/TestUsers.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar obtener los nombres de archivo de la evaluación: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }
        }


    }
}