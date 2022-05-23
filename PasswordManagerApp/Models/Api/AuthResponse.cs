using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManagerApp.Models.Api
{
    public class AuthResponse
    {
        [JsonProperty("message")]
        public string Message
        {
            get; set;
        }
    }
}
