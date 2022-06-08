using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using PasswordManagerApp.Models;
using PasswordManagerApp.Models.Api;
using System.Security.Cryptography;
using System.Linq;

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
            // // Get запрос с телом так и не заработал
            //var request = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Get,
            //    RequestUri = new Uri(Manager.apiUrl + "users/auth"),
            //    Content = requestBody
            //};
            var response = client.PostAsync(Manager.apiUrl + "users/auth", requestBody);
            
            return JsonConvert.DeserializeObject<ApiFrame<AuthResponse>>(response.Result.Content.ReadAsStringAsync().Result);
        }
        public List<Setting> GetUserSettings(string email, string password_hashed)
        {
            User currentUser = new User { Email = email, Password = password_hashed };
            string json = JsonConvert.SerializeObject(currentUser, Formatting.None);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Manager.apiUrl + "settings/current", requestBody);
            List<Setting> settedSettings = JsonConvert.DeserializeObject<ApiFrame<List<Setting>>>(response.Result.Content.ReadAsStringAsync().Result).Data;
            foreach (SettingCode setting in GetSettingCodes(email, password_hashed))
            {
                if (!settedSettings.Where(x => x.SettingCode == setting.Code).Any())
                {
                    Setting newSetting = new Setting { SettingCode = setting.Code, Value = false, UserEmail = Manager.currentUserEmail };
                    SetSetting(new List<Setting> { newSetting });
                    settedSettings.Add(newSetting);
                }
            }
            return settedSettings;
        }

        public List<SettingCode> GetSettingCodes(string email, string password_hashed)
        {
            User currentUser = new User { Email = email, Password = password_hashed };
            string json = JsonConvert.SerializeObject(currentUser, Formatting.None);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Manager.apiUrl + "settings/list", requestBody);
            return JsonConvert.DeserializeObject<ApiFrame<List<SettingCode>>>(response.Result.Content.ReadAsStringAsync().Result).Data;
        }
        public ApiFrame<string> SetSetting(List<Setting> settings)
        {
            List<ApiSetting> postSettings = new List<ApiSetting>();
            foreach (Setting setting in settings)
            {
                postSettings.Add(new ApiSetting { Setting = setting.SettingCode, Value = setting.Value });
            }

            RequestBody<List<ApiSetting>> request = new RequestBody<List<ApiSetting>>
            {
                Auth = new User { Email = Manager.currentUserEmail, Password = string.Join(" ", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Manager.currentUserPassword))) },
                Data = postSettings
            };

            string json = JsonConvert.SerializeObject(request, Formatting.None);
            Console.WriteLine(json);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Manager.apiUrl + "settings/new", requestBody);
            return JsonConvert.DeserializeObject<ApiFrame<string>>(response.Result.Content.ReadAsStringAsync().Result);
        }
    }
}
