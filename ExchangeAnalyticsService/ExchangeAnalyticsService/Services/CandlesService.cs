using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib.Classes;
using ExchCommonLib.Classes.Requests;
using ExchCommonLib.Classes.Responses;
using ExchCommonLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Services
{
    public class CandlesService : ICandlesService
    {
        private readonly IRatesRepository ratesRepository;

        uint maxDiffDays = 30;

        public CandlesService(IRatesRepository ratesRepository)
        {
            this.ratesRepository = ratesRepository;
        }

        public CandlesResponse GetCandles(CandlesRequest candlesRequest)
        {
            if (candlesRequest == null || candlesRequest.InstrumentId == 0)
                return new CandlesResponse();

            if (candlesRequest.DateStart == candlesRequest.DateEnd && candlesRequest.DateStart == default(DateTime))
                return new CandlesResponse();

            (candlesRequest.DateStart, candlesRequest.DateEnd) = PrepareDate(candlesRequest.DateStart, candlesRequest.DateEnd);

            var candlesFromDb = GetCandlesFromDb(candlesRequest.InstrumentId, candlesRequest.DateStart, candlesRequest.DateEnd);
            var candleInterval = GetCandlesIntervalFromStringValue(candlesRequest.Interval);

            var convertedCandels = ConvertMinuteCandlesToInterval(candlesFromDb, candleInterval);

            return new CandlesResponse() { Candles = convertedCandels };
        }

        private (DateTime, DateTime) PrepareDate(DateTime dateStart, DateTime dateEnd)
        {
            if (dateStart > dateEnd)
                dateEnd = dateStart;

            var diffDays = dateEnd.Subtract(dateStart).TotalDays;

            //if (diffDays > maxDiffDays)
            //    dateStart = dateEnd.AddDays(-maxDiffDays);

            return (dateStart, dateEnd);
        }

        private List<Candle> GetCandlesFromDb(uint instrumentId, DateTime dateStart, DateTime dateEnd)
        {
            return ratesRepository.GetRatesFromDb(instrumentId, dateStart, dateEnd);
        }


        private CandlesInterval GetCandlesIntervalFromStringValue(string intervalStr)
        {
            switch (intervalStr)
            {
                case "1min": return CandlesInterval.Min;
                case "5min": return CandlesInterval.FiveMin;
                case "15min": return CandlesInterval.FifteenMin;
                case "30min": return CandlesInterval.ThirtyMin;
                case "hour": return CandlesInterval.Hour;
                case "day": return CandlesInterval.Day;
                case "week": return CandlesInterval.Week;
                case "month": return CandlesInterval.Month;
                default: return CandlesInterval.Min;
            }
        }


        private List<Candle> ConvertMinuteCandlesToInterval(List<Candle> minuteCandles, CandlesInterval candlesInterval)
        {
            if (candlesInterval == CandlesInterval.Min)
                return minuteCandles;

            return minuteCandles;
        }


    }
}
