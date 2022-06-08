using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

using PasswordManagerApp.Controllers;
using System.Security.Cryptography;
using System.Linq;

namespace PasswordManagerApp.Models
{
    public class Setting
    {
        private UsersController _usersCtrl = new UsersController();
        [JsonProperty("settingCode")]
        public string SettingCode { get; set; }
        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }
        [JsonProperty("value")]
        public bool Value { get; set; }
        [JsonIgnore]
        public string Name {
            get 
            {
                return _usersCtrl.GetSettingCodes(Manager.currentUserEmail, string.Join(" ", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Manager.currentUserPassword)))).Where(x => x.Code == SettingCode).FirstOrDefault().Name;
            }
        }
    }
}
