using DbWrapperCore;
using ExchCommonLib;
using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchCommonLib.Enums;

namespace ExchangeAnalyticsService.IRepositories
{
    public interface IRatesRepository
    {

        List<Candle> GetRatesFromDb(uint instrumentId, CandlesInterval interval, DateTime dateStart, DateTime dateEnd);
        DateTime? GetLastCandleDtForInstrumentFromDb(uint instrumentId);

        void SaveCandlesToDb(uint instrumentId, List<Candle> candles);
    }
}
