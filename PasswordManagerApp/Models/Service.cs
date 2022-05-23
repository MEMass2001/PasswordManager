using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace PasswordManagerApp.Models
{
    public class Service
    {
        [JsonProperty("id")]
        public int Id
        {
            get; set;
        }
        [JsonProperty("name")]
        public string Name
        {
            get; set;
        }
        [JsonProperty("image")]
        public string Image
        {
            get; set;
        }
    }
}