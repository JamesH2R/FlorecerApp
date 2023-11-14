using FluorecerApp_Client.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

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
                    multiContent.Add(fileContent, "file", fileName);

                    string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/AssignEvaluation";

                    HttpResponseMessage response = await client.PostAsync(url, multiContent);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return "Evaluación asignada con éxito.";
                    }

                    return "No se pudo asignar la evaluación.";
                }
                catch (Exception ex)
                {
                    return ex.Message;
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
                        return "No se pudo descargar la evaluación.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error al descargar la evaluación: {ex.Message}";
            }
        }



    }
}