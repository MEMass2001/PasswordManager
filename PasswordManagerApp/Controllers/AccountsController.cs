using Newtonsoft.Json;
using PasswordManagerApp.Models;
using PasswordManagerApp.Models.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManagerApp.Controllers
{
    public class AccountsController
    {
        private static readonly HttpClient client = new HttpClient();
        public ApiFrame<List<Service>> GetServices()
        {
            User newUser = new User { Email = Manager.currentUserEmail, Password = string.Join(" ", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Manager.currentUserPassword)))};
            string json = JsonConvert.SerializeObject(newUser, Formatting.None);
            Console.WriteLine(json);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Manager.apiUrl + "accounts/services", requestBody);
            string result = response.Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ApiFrame<List<Service>>>(result);
        }
        public ApiFrame<string> 
    }
}
