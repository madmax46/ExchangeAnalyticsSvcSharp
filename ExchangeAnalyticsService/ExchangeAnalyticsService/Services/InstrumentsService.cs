using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Models;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib;
using ExchCommonLib.Classes.Exchange;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Services
{
    public class InstrumentsService : IInstrumentsService
    {

        private readonly IInstrumentsRepository instrumentsRepository;
        private readonly IParsersRepository parsersRepository;
        private readonly ILogger<InstrumentsService> logger;

        public InstrumentsService(IInstrumentsRepository instrumentsRepository, IParsersRepository parsersRepository, ILogger<InstrumentsService> logger)
        {
            this.instrumentsRepository = instrumentsRepository;
            this.parsersRepository = parsersRepository;
            this.logger = logger;
        }

        public ExchangeMarkets GetMarketsFromDb()
        {
            return instrumentsRepository.GetMarketsFromDb();
        }

        public InstrumentsResponse GetParsedInstruments()
        {
            try
            {
                var allParsers = parsersRepository.GetAll();

                InstrumentsResponse response = new InstrumentsResponse()
                {
                    Instruments = allParsers.Select(r => r.Instrument).ToList()
                };

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка!!!");
                return null;
            }
        }
    }
}
