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
        public ApiFrame<string> AddAccount(string title, string login_ciph, string password_ciph, int service)
        {
            SetAccount newAccount = new SetAccount
            {
                Title = title,
                Login = login_ciph,
                Password = password_ciph,
                Service = service
            };

            RequestBody<SetAccount> request = new RequestBody<SetAccount>
            {
                Auth = new User { Email = Manager.currentUserEmail, Password = string.Join(" ", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Manager.currentUserPassword))) },
                Data = newAccount
            };

            string json = JsonConvert.SerializeObject(request, Formatting.None);
            Console.WriteLine(json);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Manager.apiUrl + "accounts/create", requestBody);
            string result = response.Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ApiFrame<string>>(result);
        }
        public ApiFrame<List<Account>> GetAccounts()
        {
            User newUser = new User { Email = Manager.currentUserEmail, Password = string.Join(" ", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Manager.currentUserPassword))) };
            string json = JsonConvert.SerializeObject(newUser, Formatting.None);
            Console.WriteLine(json);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Manager.apiUrl + "accounts/list", requestBody);
            string result = response.Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ApiFrame<List<Account>>>(result);
        }
        public ApiFrame<Account> GetAccount(int accountId)
        {
            User newUser = new User { Email = Manager.currentUserEmail, Password = string.Join(" ", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Manager.currentUserPassword))) };
            string json = JsonConvert.SerializeObject(newUser, Formatting.None);
            Console.WriteLine(json);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Manager.apiUrl + $"accounts/{accountId}", requestBody);
            string result = response.Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ApiFrame<Account>>(result);
        }
        public ApiFrame<string> EditAccount(string title, string login_ciph, string password_ciph, int service, int accountId)
        {
            SetAccount newAccount = new SetAccount
            {
                Title = title,
                Login = login_ciph,
                Password = password_ciph,
                Service = service
            };

            RequestBody<SetAccount> request = new RequestBody<SetAccount>
            {
                Auth = new User { Email = Manager.currentUserEmail, Password = string.Join(" ", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Manager.currentUserPassword))) },
                Data = newAccount
            };
            string json = JsonConvert.SerializeObject(request, Formatting.None);
            Console.WriteLine(json);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PutAsync(Manager.apiUrl + $"accounts/{accountId}", requestBody);
            string result = response.Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ApiFrame<string>>(result);
        }
    }
}
