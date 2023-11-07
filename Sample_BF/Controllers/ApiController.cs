using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Sample_BF.AzureStorage;

namespace Sample_BF.Controllers
{
    public class ApiController
    {

        private static HttpClient sharedClient = new HttpClient();

        [HttpGet]
        [Route("/test")]
        async public Task<string> Get(string query)
        {
                return "Servidor Backend iniciado. Envia la pregunta en el parámetro query de la url.";
        }
    }

}

