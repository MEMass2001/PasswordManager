using Newtonsoft.Json;
using PasswordManagerApp.Classes;
using PasswordManagerApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordManagerApp.Models
{
    public partial class Account
    {
        private AccountsController acntCtrl = new AccountsController();
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("accountLogin")]
        public string AccountLogin { get; set; }
        [JsonProperty("accountPassword")]
        public string AccountPassword { get; set; }
        [JsonProperty("serviceId")]
        public int ServiceId { get; set; }
        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }
        public string AccountImage { 
            get 
            {
                List<Service> services = acntCtrl.GetServices().Data;
                return $"@drawable/{services.Where(x => x.Id == ServiceId).FirstOrDefault().Image}";
            } 
        }
        public string DecipheredLogin
        {
            get
            {
                return IdeaCipher.cryptString(AccountLogin, Manager.currentUserPassword, false);
            }
        }
        public string DecipheredPassword
        {
            get
            {
                return IdeaCipher.cryptString(AccountPassword, Manager.currentUserPassword, false);
            }
        }
    }
}
