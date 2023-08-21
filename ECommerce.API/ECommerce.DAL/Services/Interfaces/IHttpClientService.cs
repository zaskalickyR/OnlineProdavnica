using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<string> PostDataToApi(string url, string jsonData);
    }
}
