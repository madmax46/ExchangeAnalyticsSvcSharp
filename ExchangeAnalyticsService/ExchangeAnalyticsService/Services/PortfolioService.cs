using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib.Analytics;
using ExchCommonLib.Classes.Operations;
using ExchCommonLib.Classes.Requests;
using ExchCommonLib.Classes.UserPortfolio;
using Microsoft.Extensions.Logging;

namespace ExchangeAnalyticsService.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ILogger<PortfolioService> logger;

        private readonly IPortfolioRepository portfolioRepository;
        private readonly IOperationsRepository operationsRepository;
        private readonly ICandlesService candlesService;
        private readonly IAnalyticsService analyticsService;
        private readonly IInstrumentsService instrumentsService;

        public PortfolioService(IPortfolioRepository portfolioRepository, IOperationsRepository operationsRepository, ILogger<PortfolioService> logger, ICandlesService candlesService, IAnalyticsService analyticsService, IInstrumentsService instrumentsService)
        {
            this.logger = logger;
            this.portfolioRepository = portfolioRepository;
            this.operationsRepository = operationsRepository;
            this.candlesService = candlesService;
            this.analyticsService = analyticsService;
            this.instrumentsService = instrumentsService;
        }


        public Portfolio LoadUserPortfolio(uint userId)
        {
            try
            {
                var instrumentsResponse = instrumentsService.GetParsedInstruments();
                var instDict = instrumentsResponse.Instruments.ToDictionary(r => (uint)r.FinamEmitentIDInt, r => r);
                var portfolio = portfolioRepository.LoadUserPortfolio(userId);

                if (!portfolio.Positions.Any())
                    return portfolio;
                //удаляю те, которых уже нету в базе
                portfolio.Positions.RemoveAll(r =>
                {
                    if (!instDict.TryGetValue(r.InstrumentId, out var value)) return true;
                    r.Name = value.Name;
                    return false;
                });

                foreach (var onePosition in portfolio.Positions)
                {
                    try
                    {
                        var candlesResponse = candlesService.GetLastCandleForInstrument(new LastCandleRequest(onePosition.InstrumentId, "min"));
                        if (candlesResponse?.Candles?.Any() != true)
                        {
                            onePosition.CurСost = onePosition.AvgPrice * onePosition.RemCount;
                            continue;
                        }

                        var lastCandle = candlesResponse.Candles.Last();
                        onePosition.CurPrice = lastCandle.Close;
                        onePosition.CurСost = onePosition.CurPrice.Value * onePosition.RemCount;
                        onePosition.Income = onePosition.CurСost - onePosition.AvgPrice * onePosition.RemCount;

                        var analysisResponse = analyticsService.GetPredictionFor(new PredictionRequest()
                        { InstrumentId = onePosition.InstrumentId, Interval = "30min" });

                        if (!analysisResponse.Predictions.Any())
                        {
                            onePosition.Analytics = "--";
                            continue;
                        }

                        var analytics = new ConsolidateInstrumentsAnalysis(analysisResponse.Predictions);
                        onePosition.Analytics = analytics.Summary;

                        // TotalAmount
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "");
                    }
                }

                if (portfolio.Positions.Any())
                    portfolio.TotalAmount = portfolio.Positions.Sum(r => r.CurСost);

                return portfolio;
            }
            catch (Exception e)
            {
                logger.LogError(e, "");
                return null;
            }
        }

        public OperationsHistory GetUserOperationsHistory(uint userId)
        {
            try
            {
                return operationsRepository.GetUserOperationsHistory(userId);
            }
            catch (Exception e)
            {
                logger.LogError(e, "");
                return null;
            }
        }

        public bool SaveUserOperationToDb(uint userId, MarketOperation operation)
        {
            try
            {
                return operationsRepository.SaveUserOperationToDb(userId, operation);
            }
            catch (Exception e)
            {
                logger.LogError(e, "");
                return false;
            }
        }

        public bool DeleteUserOperation(uint userId, uint operationId)
        {
            try
            {
                return operationsRepository.DeleteUserOperation(userId, operationId);
            }
            catch (Exception e)
            {
                logger.LogError(e, "");
                return false;
            }
        }
    }
}
