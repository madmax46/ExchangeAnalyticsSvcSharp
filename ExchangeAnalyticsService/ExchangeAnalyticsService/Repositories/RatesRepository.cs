using DbWrapperCore;
using ExchangeAnalyticsService.IRepositories;
using ExchCommonLib;
using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ExchCommonLib.Enums;
using ExchCommonLib.Utils;
using Microsoft.EntityFrameworkCore.Internal;

namespace ExchangeAnalyticsService.Repositories
{

    public class RatesRepository : IRatesRepository
    {
        private readonly IDBProvider dbProvider;


        public RatesRepository(IDBProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }


        public List<Candle> GetRatesFromDb(uint instrumentId, CandlesInterval interval, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                var table = GetRatesTableFromDb(instrumentId, interval, dateStart, dateEnd);
                var rates = ParseRatesFromTable(table);

                return rates;
            }
            catch
            {
                return new List<Candle>();
            }
        }

        public DateTime? GetLastCandleDtForInstrumentFromDb(uint instrumentId)
        {
            try
            {
                var table = dbProvider.ProcedureByName("svc_getMaxCandleDtForInstrument", instrumentId);
                if (!table.Rows.Any())
                    return null;
                if (Convert.IsDBNull(table.Rows[0][0]))
                    return null;


                return Convert.ToDateTime(table.Rows[0][0]);

            }
            catch
            {
                return null;
            }

        }



        private DataTable GetRatesTableFromDb(uint instrumentId, CandlesInterval interval, DateTime dateStart, DateTime dateEnd)
        {
            DataTable table = new DataTable();
            try
            {
                table = dbProvider.ProcedureByName("svc_getRates", instrumentId, interval, dateStart, dateEnd);
            }
            catch (Exception ex)
            {

            }
            return table;
        }

        private List<Candle> ParseRatesFromTable(DataTable ratesTable)
        {
            List<Candle> rates = new List<Candle>();

            foreach (DataRow oneRow in ratesTable.Rows)
            {
                rates.Add(Candle.FromRow(oneRow));
            }

            return rates;
        }


        public void SaveCandlesToDb(uint instrumentId, List<Candle> candles)
        {
            var interval = candles.First().Interval;
            SaveCandlesToDb(instrumentId, candles, interval);
        }

        private bool SaveCandlesToDb(uint instrumentId, List<Candle> candles, CandlesInterval interval)
        {
            if (candles?.Any() != true)
                return true;


            bool res = true;

            try
            {
                List<DateTime> ratesDates = candles.Select(r => r.Date.Date).Distinct().OrderBy(r => r).ToList();
                var countExistRows = DeleteMinAggRowsByDate(instrumentId, ratesDates, interval);

                var aggregatedRates = candles;

                List<string> valuesPart = new List<string>();

                aggregatedRates.ForEach(oneRate => { valuesPart.Add(RateAggregationToSqlValue(instrumentId, oneRate, interval)); });

                List<List<string>> groupsByCount = new List<List<string>>();

                if (valuesPart.Count <= 10000)
                {
                    groupsByCount.Add(valuesPart);
                }
                else
                {
                    groupsByCount = valuesPart.ChunkBy(10000);
                }

                foreach (var oneGroup in groupsByCount)
                {
                    string queryInsertRates = $"INSERT INTO quotes_aggregations (`idInstrument`, `date`, `open`, `high`, `low`, `close`, `volume`, `intervalAgg`) VALUES {string.Join(",", oneGroup)}";
                    int countInsert = dbProvider.Execute(queryInsertRates);
                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }

        private long DeleteMinAggRowsByDate(uint finamEmitentID, List<DateTime> ratesDates, CandlesInterval interval)
        {
            int allCountDeleted = 0;
            int countRows = 0;

            string minDate = dbProvider.ToSqlParam(ratesDates.Min());
            string maxDate = dbProvider.ToSqlParam(ratesDates.Max().AddDays(1).AddSeconds(-1));
            string intervalSql = dbProvider.ToSqlParam(interval);
            string queryDeleteOld = $"delete from quotes_aggregations where idInstrument = {finamEmitentID} and intervalAgg = {intervalSql} and date BETWEEN {minDate} and {maxDate}  limit 5000";

            do
            {
                countRows = dbProvider.Execute(queryDeleteOld);
                allCountDeleted += countRows;
            } while (countRows == 5000);
            return allCountDeleted;
        }


        public string RateAggregationToSqlValue(uint idInstrument, Candle oneRate, CandlesInterval interval)
        {
            return $"({dbProvider.ToSqlParam(idInstrument)}," +
                   $"{dbProvider.ToSqlParam(oneRate.Date)}," +
                   $"{dbProvider.ToSqlParam(oneRate.Open)}," +
                   $"{dbProvider.ToSqlParam(oneRate.High)}," +
                   $"{dbProvider.ToSqlParam(oneRate.Low)}," +
                   $"{dbProvider.ToSqlParam(oneRate.Close)}," +
                   $"{dbProvider.ToSqlParam(oneRate.Volume)}," +
                   $"{dbProvider.ToSqlParam(interval)})";
        }

    }
}
