using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManagerApp.Models.Api
{
    public class ApiSetting
    {
        [JsonProperty("setting")]
        public string Setting { get; set; }
        [JsonProperty("value")]
        public bool Value { get; set; }
    }
}
