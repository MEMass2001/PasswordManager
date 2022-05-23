using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManagerApp.Models.Api
{
    public class ApiFrame<T>
    {
        [JsonProperty("status")]
        public string Status
        {
            get; set;
        }
        [JsonProperty("data")]
        public T Data
        {
            get; set;
        }
    }
}
