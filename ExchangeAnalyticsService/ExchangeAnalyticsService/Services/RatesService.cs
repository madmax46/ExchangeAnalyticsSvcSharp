using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Services
{
    public class RatesService : IRatesService
    {
        public IRatesRepository RatesRepository { get; private set; }

        uint maxDiffDays = 30;

        public RatesService(IRatesRepository ratesRepository)
        {
            RatesRepository = ratesRepository;
        }

        public List<Rate> GetRatesFromDb(uint instrumentId, DateTime dateStart, DateTime dateEnd)
        {
            if (instrumentId == 0)
                return new List<Rate>();
            if (dateStart == dateEnd && dateStart == default(DateTime))
                return new List<Rate>();

            if (dateStart > dateEnd)
                dateEnd = dateStart;

            var diffDays = dateEnd.Subtract(dateStart).TotalDays;

            if (diffDays > maxDiffDays)
                dateStart = dateEnd.AddDays(-maxDiffDays);


            return RatesRepository.GetRatesFromDb(instrumentId, dateStart, dateEnd);
        }
    }
}
