using DbWrapperCore;
using ExchangeAnalyticsService.IRepositories;
using ExchCommonLib;
using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;

namespace ExchangeAnalyticsService.Repositories
{

    public class RatesRepository : IRatesRepository
    {
        public IDBProvider dbProvider { get; }


        public RatesRepository(IDBProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }


        public List<Candle> GetRatesFromDb(uint instrumentId, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                var table = GetRatesTableFromDb(instrumentId, dateStart, dateEnd);
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

                return Convert.ToDateTime(table.Rows[0][0]);
            }
            catch
            {
                return null;
            }

        }


        private DataTable GetRatesTableFromDb(uint instrumentId, DateTime dateStart, DateTime dateEnd)
        {
            DataTable table = new DataTable();
            try
            {
                table = dbProvider.ProcedureByName("svc_getRates", instrumentId, dateStart, dateEnd);
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


    }
}
