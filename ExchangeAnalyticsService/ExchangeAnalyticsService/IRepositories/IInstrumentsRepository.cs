using DbWrapperCore;
using ExchCommonLib;
using ExchCommonLib.Classes.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.IRepositories
{
    public interface IInstrumentsRepository
    {
        IDBProvider dbProvider { get; }

        ExchangeMarkets GetMarketsFromDb();
    }
}
