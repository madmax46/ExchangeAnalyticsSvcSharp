using ExchangeAnalyticsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Services.Interfaces
{
    public interface IAccountService
    {
        AuthenticateResponse Login(LoginInfo loginInfo);
    }
}
