using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Models
{
    public class AuthenticateResponse
    {
     
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
        [JsonProperty(PropertyName = "login")]
        public string Login { get; set; }

        public AuthenticateResponse(string token, string login)
        {
            Token = token;
            Login = login;
        }

    }
}
