using ExchangeAnalyticsService.Models;
using ExchCommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.IRepositories
{
    public interface IAccountRepository
    {
        IDBProvider dbProvider { get; }

        Person GetUserByLoginAndPassword(string login, string password);

    }
}
