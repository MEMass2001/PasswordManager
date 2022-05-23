using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManagerApp.Models.Api
{
    public class RequestBody<T>
    {
        [JsonProperty("auth")]
        public User Auth { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
