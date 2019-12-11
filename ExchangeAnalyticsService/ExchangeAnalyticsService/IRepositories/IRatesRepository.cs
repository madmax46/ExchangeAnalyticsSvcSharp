using DbWrapperCore;
using ExchCommonLib;
using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.IRepositories
{
    public interface IRatesRepository
    {
        IDBProvider dbProvider { get; }

        List<Candle> GetRatesFromDb(uint instrumentId, DateTime dateStart, DateTime dateEnd);
    }
}
