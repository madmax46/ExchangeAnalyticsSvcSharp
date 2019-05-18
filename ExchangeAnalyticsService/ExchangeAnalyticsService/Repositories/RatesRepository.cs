﻿using ExchangeAnalyticsService.IRepositories;
using ExchCommonLib;
using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Repositories
{

    public class RatesRepository : IRatesRepository
    {
        public IDBProvider dbProvider { get; }

        uint maxDiffDays = 30;

        public RatesRepository(IDBProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }


        public List<Rate> GetRatesFromDb(uint instrumentId, DateTime dateStart, DateTime dateEnd)
        {
            if (instrumentId == 0)
                return new List<Rate>();
            if (dateStart == dateEnd && dateStart == default(DateTime))
                return new List<Rate>();

            try
            {
                if (dateStart > dateEnd)
                    dateEnd = dateStart;

                var diffDays = dateEnd.Subtract(dateStart).TotalDays;

                if (diffDays > maxDiffDays)
                    dateStart = dateEnd.AddDays(-maxDiffDays);

                var table = GetRatesTableFromDb(instrumentId, dateStart, dateEnd);

                var rates = ParseRatesFromTable(table);

                return rates;
            }
            catch
            {
                return new List<Rate>();
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

        private List<Rate> ParseRatesFromTable(DataTable ratesTable)
        {
            List<Rate> rates = new List<Rate>();

            foreach (DataRow oneRow in ratesTable.Rows)
            {
                rates.Add(Rate.FromRow(oneRow));
            }

            return rates;
        }


    }
}