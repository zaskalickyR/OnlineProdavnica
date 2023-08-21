using ECommerce.DAL.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Services.Implementations
{
    public class HttpClientService : IHttpClientService
    {


        public HttpClientService()
        {

        }
        public async Task<string> PostDataToApi(string url, string jsonData)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();
                        return responseJson;
                    }
                    else
                    {
                        Console.WriteLine("Došlo je do problema prilikom slanja podataka na API. Statusni kod: " + response.StatusCode);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Došlo je do greške prilikom slanja zahtjeva: " + e.Message);
            }

            return null; // Ukoliko dođe do greške, vraćamo null
        }

    }
}
