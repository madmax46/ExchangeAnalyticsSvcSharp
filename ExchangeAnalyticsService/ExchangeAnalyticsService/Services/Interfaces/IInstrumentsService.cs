using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Models;
using ExchCommonLib;
using ExchCommonLib.Classes.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Services.Interfaces
{
    public interface IInstrumentsService
    {
        ExchangeMarkets GetMarketsFromDb();
        InstrumentsResponse GetParsedInstruments();
    }
}
