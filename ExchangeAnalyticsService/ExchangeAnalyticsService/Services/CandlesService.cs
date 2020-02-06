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
using ExchCommonLib.CandlesUtils;

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

        public CandlesResponse GetCandles(CandlesRequest candlesRequest, bool isNeedLastCandleInfo = false)
        {
            var candles = GetCandlesByRequest(candlesRequest);

            if (isNeedLastCandleInfo)
            {
                var lastCandle = GetLastCandleForInstrument(new LastCandleRequest(candlesRequest.InstrumentId, "min"));
                candles.LastCandle = lastCandle.LastCandle;
            }

            return candles;
        }

        private CandlesResponse GetCandlesByRequest(CandlesRequest candlesRequest)
        {
            if (candlesRequest == null || candlesRequest.InstrumentId == 0)
                return new CandlesResponse();

            if (candlesRequest.DateStart == candlesRequest.DateEnd && candlesRequest.DateStart == default(DateTime))
                return new CandlesResponse();

            (candlesRequest.DateStart, candlesRequest.DateEnd) = PrepareDate(candlesRequest.DateStart, candlesRequest.DateEnd);

            var candleInterval = GetCandlesIntervalFromStringValue(candlesRequest.Interval);
            var candlesFromDb = GetCandlesFromDb(candlesRequest.InstrumentId, candleInterval, candlesRequest.DateStart, candlesRequest.DateEnd);

            //var convertedCandles = ConvertMinuteCandlesToInterval(candlesFromDb, candleInterval);

            //return new CandlesResponse() {Candles = convertedCandles, LastCandle = lastCandle.Candles.FirstOrDefault()};
            //return new CandlesResponse() { Candles = convertedCandles };
            return new CandlesResponse() { Candles = candlesFromDb };
        }

        public CandlesResponse GetLastCandleForInstrument(LastCandleRequest lastCandleRequest)
        {
            var dt = ratesRepository.GetLastCandleDtForInstrumentFromDb(lastCandleRequest.InstrumentId);
            if (!dt.HasValue)
                return new CandlesResponse();

            var dtStart = dt.Value.AddDays(-7);
            var dtEnd = dt.Value.AddDays(7);

            var candlesResponse = GetCandlesByRequest(new CandlesRequest() { DateStart = dtStart, DateEnd = dtEnd, InstrumentId = lastCandleRequest.InstrumentId, Interval = lastCandleRequest.Interval });
            if (!candlesResponse.Candles.Any())
                return new CandlesResponse();

            var last = candlesResponse.Candles.Last();
            var lastList = new List<Candle>() { last };
            candlesResponse.Candles = lastList;
            candlesResponse.LastCandle = last;

            return candlesResponse;
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

        private List<Candle> GetCandlesFromDb(uint instrumentId, CandlesInterval interval, DateTime dateStart, DateTime dateEnd)
        {
            return ratesRepository.GetRatesFromDb(instrumentId, interval, dateStart, dateEnd);
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


            return CommonCandlesUtils.AggregateMinCandlesToInterval(minuteCandles, candlesInterval);
        }

        public void SaveAggCandlesToDb(CandlesRequest candlesRequest)
        {
            candlesRequest.Interval = "1min";
            var minCandles = GetCandlesByRequest(candlesRequest);
            var enumArr = Enum.GetValues(typeof(CandlesInterval));

            foreach (var oneInterval in enumArr)
            {
                var intVal = Convert.ToUInt32(oneInterval);
                var enumVal = (CandlesInterval)oneInterval;
                if (enumVal == CandlesInterval.Min)
                    continue;

                var convertedCandles = ConvertMinuteCandlesToInterval(minCandles.Candles, enumVal);

                ratesRepository.SaveCandlesToDb(candlesRequest.InstrumentId, convertedCandles);
            }

        }
    }
}
