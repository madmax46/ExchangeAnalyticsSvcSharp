using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib;
using ExchCommonLib.Classes.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Services
{
    public class InstrumentsService : IInstrumentsService
    {

        public IInstrumentsRepository InstrumentsRepository { get; }

        public InstrumentsService(IInstrumentsRepository instrumentsRepository)
        {
            InstrumentsRepository = instrumentsRepository;
        }

        public ExchangeMarkets GetMarketsFromDb()
        {
            return InstrumentsRepository.GetMarketsFromDb();
        }
    }
}
