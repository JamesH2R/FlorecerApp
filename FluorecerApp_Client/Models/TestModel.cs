using FluorecerApp_Client.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace FluorecerApp_Client.Models
{
    public class TestModel
    {
        //ADMIN

        public async Task<string> AssignEvaluation(MedicalTestsEnt test, Stream fileStream, string fileName)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    string testJson = Newtonsoft.Json.JsonConvert.SerializeObject(test);
                    StringContent jsonContent = new StringContent(testJson);
                    multiContent.Add(jsonContent, "test");

                    StreamContent fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"file\"",
                        FileName = "\"" + fileName + "\""
                    };
                    multiContent.Add(fileContent);

                    string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/AssignEvaluation";

                    HttpResponseMessage response = await client.PostAsync(url, multiContent);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return "Evaluación asignada con éxito.";
                    }
                    else
                    {
                        return "No se pudo asignar la evaluación.";
                    }
                }
                catch (Exception ex)
                {
                    return $"Error al asignar la evaluación: {ex.Message}";
                }
            }
        }

        public async Task<List<UsersEnt>> GetDropdownLastNames()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/getUserDropdown";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UsersEnt>>(jsonResult);
                        return result;
                    }
                    else
                    {
                        return new List<UsersEnt>();
                    }
                }
                catch
                {
                    // Manejar el error según tus necesidades
                    return new List<UsersEnt>();
                }
            }
        }

        public async Task<UsersEnt> GetUserById(long userId)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    string url = ConfigurationManager.AppSettings["urlApi"].ToString() + $"api/getUserById/{userId}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<UsersEnt>(jsonResult);
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        public async Task<string> DeleteEvaluations(long userId)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // URL del API
                    string url = ConfigurationManager.AppSettings["urlApi"].ToString() + $"api/DeleteEvaluations/{userId}";

                    // Llamada a la API y manejo de la respuesta
                    HttpResponseMessage response = await client.DeleteAsync(url);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return "Evaluaciones eliminadas con éxito.";
                    }
                    else
                    {
                        return "No se pudo eliminar las evaluaciones.";
                    }
                }
                catch (Exception ex)
                {
                    return $"Error al eliminar las evaluaciones: {ex.Message}";
                }
            }

        }

        public List<TestResultsEnt> TestUsersDone()
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/GetAllFileNames";

                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<List<TestResultsEnt>>().Result;
                }

                return new List<TestResultsEnt>();
            }
        }

        public async Task<string> DownloadTestResult(long ResultId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // URL del API
                    string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/DownloadTestResult/" + ResultId;

                    // Llamada a la API y manejo de la respuesta
                    HttpResponseMessage response = await client.GetAsync(url);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return "Evaluación descargada con éxito.";
                    }
                    else
                    {
                        return "No se pudo descargar la evaluación.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error al descargar la evaluación: {ex.Message}";
            }
        }

        public async Task<string> DeleteTestResult(long ResultId)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // URL del API
                    string url = ConfigurationManager.AppSettings["urlApi"].ToString() + $"api/DeleteTestResult/{ResultId}";

                    // Llamada a la API y manejo de la respuesta
                    HttpResponseMessage response = await client.DeleteAsync(url);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return "Evaluación eliminada con éxito.";
                    }
                    else
                    {
                        return "No se pudo eliminar la evaluación.";
                    }
                }
                catch (Exception ex)
                {
                    return $"Error al eliminar las evaluaciones: {ex.Message}";
                }
            }

        }

        //USUARIOS
        public async Task<string> DownloadEvaluation(long userId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // URL del API
                    string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/DownloadEvaluation/" + userId;

                    // Llamada a la API y manejo de la respuesta
                    HttpResponseMessage response = await client.GetAsync(url);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return "Evaluación descargada con éxito.";
                    }
                    else
                    {
                        return "Evaluación descargada anteriormente. Revise sus descargas.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error al descargar la evaluación: {ex.Message}";
            }
        }

        public async Task<string> SendResult(HttpPostedFileBase file)
        {
            try
            {
                using (var client = new HttpClient())
                using (var content = new MultipartFormDataContent())
                {
                    // Configuración del contenido del formulario
                    content.Headers.ContentType.MediaType = "multipart/form-data";

                    // Agregar el archivo al formulario
                    var fileContent = new StreamContent(file.InputStream);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"file\"",
                        FileName = $"\"{file.FileName}\""
                    };
                    content.Add(fileContent);

                    // URL del API
                    string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/SendResult";

                    // Llamada a la API y manejo de la respuesta
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return "Evaluación enviada con éxito.";
                    }
                    else
                    {
                        return "No se pudo enviar la evaluación.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error al enviar la evaluación: {ex.Message}";
            }
        }

        public async Task<List<string>> GetUserEvaluationNames(long userId)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    string url = ConfigurationManager.AppSettings["urlApi"].ToString() + $"api/GetUserEvaluationNames/{userId}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(jsonResult);
                        return result;
                    }
                    else
                    {
                        return new List<string>();
                    }
                }
                catch
                {
                    return new List<string>();
                }
            }
        }


    }
}