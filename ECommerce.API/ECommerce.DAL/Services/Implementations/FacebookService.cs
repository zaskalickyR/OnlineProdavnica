using ECommerce.DAL.DTO.User.DataIn;
using ECommerce.DAL.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Services.Implementations
{
    public class FacebookService : IFacebookService
    {
        private readonly HttpClient _httpClient;

        public FacebookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FacebookUserData> GetUserData(string accessToken)
        {
            var requestUrl = $"https://graph.facebook.com/v13.0/me?fields=id,email,name,picture&access_token={accessToken}";

            var response = await _httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve user data from Facebook.");
            }

            var content = await response.Content.ReadAsStringAsync();
            FacebookUserData userData = JsonConvert.DeserializeObject<FacebookUserData>(content);
            return userData;
        }
    }
}
