using ExchangeAnalyticsService.IRepositories;
using ExchCommonLib.Classes;
using ExchCommonLib.Classes.Requests;
using ExchCommonLib.Classes.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Services.Interfaces
{
    public interface ICandlesService
    {
        CandlesResponse GetCandles(CandlesRequest candlesRequest, bool isNeedLastCandleInfo = false);
        CandlesResponse GetLastCandleForInstrument(LastCandleRequest lastCandleRequest);

    }
}
