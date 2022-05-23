using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using PasswordManagerApp.Models;
using PasswordManagerApp.Models.Api;

namespace PasswordManagerApp.Controllers
{
    public class UsersController
    {
        private static readonly HttpClient client = new HttpClient();
        public ApiFrame<AuthResponse> SignUpUser(string email, string password_hashed)
        {
            User newUser = new User { Email = email, Password = password_hashed };
            string json = JsonConvert.SerializeObject(newUser, Formatting.None);
            Console.WriteLine(json);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Manager.apiUrl + "users/signup", requestBody);
            return JsonConvert.DeserializeObject<ApiFrame<AuthResponse>>(response.Result.Content.ReadAsStringAsync().Result);
        }
        public ApiFrame<AuthResponse> AuthUser(string email, string password_hashed)
        {
            User currentUser = new User { Email = email, Password = password_hashed };
            string json = JsonConvert.SerializeObject(currentUser, Formatting.None);
            Console.WriteLine(json);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            //var request = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Get,
            //    RequestUri = new Uri(Manager.apiUrl + "users/auth"),
            //    Content = requestBody
            //};
            var response = client.PostAsync(Manager.apiUrl + "users/auth", requestBody);
            
            return JsonConvert.DeserializeObject<ApiFrame<AuthResponse>>(response.Result.Content.ReadAsStringAsync().Result);
        }
    }
}
