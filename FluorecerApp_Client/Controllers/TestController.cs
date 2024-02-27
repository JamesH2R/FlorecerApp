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
using System.Net.Http.Headers;
using System.Configuration;
using System.Net;
using System.Web.UI.WebControls.WebParts;

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

                var user = await model.GetUserById(selectedUserId);
                if (user != null)
                {
                    test.UserId = user.UserId;
                    test.LastName = user.LastName;

                    var result = await model.DeleteEvaluations(test.UserId);

                    if (result != null)
                    {
                        TempData["ResultMessage"] = "Evaluación eliminada con éxito";
                    } else
                    {
                        TempData["ErrorMessage"] = "El usuario no tiene evaluaciones pendientes";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "No se ha seleccionado un paciente";
                }

                // Indicar que se ejecutó un POST antes de este método
                TempData["PostEjecutado"] = true;

                return RedirectToAction("Assign", "Test");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar la evaluación.";
                return View("~/Views/Shared/Error.cshtml");
            }
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
                // Utilizar la instancia existente de TestModel
                HttpResponseMessage response = await model.DownloadTestResult(ResultId);

                if (response.IsSuccessStatusCode)
                {
                    // Obtener el nombre del archivo desde el encabezado Content-Disposition
                    var fileName = GetFileNameFromContentDisposition(response.Content.Headers.ContentDisposition);

                    // Ajustar el nombre del archivo para eliminar guiones bajos adicionales al final (opcional)
                    fileName = RemoveUnderscoresFromEnd(fileName);

                    // Crear un FileStreamResult para devolver el contenido del archivo
                    var fileStreamResult = new FileStreamResult(await response.Content.ReadAsStreamAsync(), response.Content.Headers.ContentType.MediaType)
                    {
                        FileDownloadName = fileName
                    };

                    return fileStreamResult;
                }
                else
                {
                    // Mostrar un mensaje de error en la vista
                    ViewBag.ErrorMessage = await response.Content.ReadAsStringAsync();
                    return View("Error");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                ViewBag.ErrorMessage = "Error al acceder al archivo: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar descargar la evaluación: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        private string GetFileNameFromContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            if (contentDisposition != null && !string.IsNullOrEmpty(contentDisposition.FileName))
            {
                return contentDisposition.FileName.Trim('"');
            }

            return null;
        }

        private string RemoveUnderscoresFromEnd(string fileName)
        {
            // Verificar si el nombre del archivo es nulo o vacío
            if (string.IsNullOrEmpty(fileName))
            {
                return fileName;
            }

            // Eliminar guiones bajos adicionales al final del nombre del archivo
            while (fileName.EndsWith("_"))
            {
                fileName = fileName.Substring(0, fileName.Length - 1);
            }

            return fileName;
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

                // Obtener las evaluaciones del usuario desde el API
                var evaluations = await model.GetUserEvaluations(userId);

                // Llamar a GetUserEvaluationNames para obtener los nombres de archivo
                var fileNames = await model.GetUserEvaluationNames(userId);

                // Configurar el ViewBag con la lista de evaluaciones para el dropdown
                ViewBag.Evaluations = new SelectList(evaluations, "TestId", "FileName");

                // Configurar el ViewBag con los nombres de archivo
                ViewBag.FileNames = fileNames;

                return View("~/Views/Test/TestUsers.cshtml");
            }
            catch (UnauthorizedAccessException ex)
            {
                ViewBag.ErrorMessage = "Error al acceder al archivo: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar obtener las evaluaciones y los nombres de archivo: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpPost]
        public async Task<ActionResult> DownloadEvaluation(long? selectedTestId)
        {
            try
            {
                if (!selectedTestId.HasValue)
                {

                    TempData["ErrorMessage"] = "No se ha elegido una evaluación";
                    return RedirectToAction("TestUsers");
                }

                long userId = (long)Session["UserId"];

                // Utilizar la instancia existente de TestModel para descargar la evaluación seleccionada
                HttpResponseMessage response = await model.DownloadEvaluation(userId, selectedTestId.Value);

                if (response.IsSuccessStatusCode)
                {
                    // Obtener el nombre del archivo desde el encabezado Content-Disposition
                    var fileName = GetFileNameFromContentDisposition(response.Content.Headers.ContentDisposition);

                    // Crear un FileStreamResult para devolver el contenido del archivo
                    var fileStreamResult = new FileStreamResult(await response.Content.ReadAsStreamAsync(), response.Content.Headers.ContentType.MediaType)
                    {
                        FileDownloadName = fileName
                    };

                    TempData.Clear(); 
                    return fileStreamResult;
                }
                else
                {
                    ViewBag.ErrorMessage = await response.Content.ReadAsStringAsync(); 
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar descargar la evaluación: " + ex.Message;
                return View("~/Views/Shared/Error.cshtml");
            }
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