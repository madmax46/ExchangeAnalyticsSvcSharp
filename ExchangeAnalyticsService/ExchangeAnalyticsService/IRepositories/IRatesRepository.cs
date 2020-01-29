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

        List<Candle> GetRatesFromDb(uint instrumentId, DateTime dateStart, DateTime dateEnd);
        DateTime? GetLastCandleDtForInstrumentFromDb(uint instrumentId);
    }
}
