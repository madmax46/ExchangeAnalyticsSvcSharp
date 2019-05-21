using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Models
{
    public class Person
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string EMail { get; set; }

        //public string Password { get; set; }
        public string Role { get; set; }
    }
}
