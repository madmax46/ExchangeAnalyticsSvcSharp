using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Auth.Classes
{
    public static class ResponseGuid
    {
        internal static string ApplicationGuid = string.Empty;

        public static void InitGuid()
        {
            ResponseGuid.ApplicationGuid = Guid.NewGuid().ToString();
        }
    }
}
