using ExchangeAnalyticsService.IRepositories;
using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Services.Interfaces
{
    public interface IRatesService
    {
        IRatesRepository RatesRepository { get; }

        List<Rate> GetRatesFromDb(uint instrumentId, DateTime dateStart, DateTime dateEnd);

    }
}
