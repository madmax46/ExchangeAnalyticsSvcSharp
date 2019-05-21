using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Models
{


    public class LoginInfo
    {
        [JsonProperty(PropertyName = "username", Required = Required.Always)]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password", Required = Required.Always)]

        public string Password { get; set; }
    }
}
