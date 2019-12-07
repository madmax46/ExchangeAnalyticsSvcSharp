using ExchangeAnalyticsService.IRepositories;
using ExchCommonLib;
using ExchCommonLib.Classes.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using DbWrapperCore;

namespace ExchangeAnalyticsService.Repositories
{
    public class InstrumentsRepository : IInstrumentsRepository
    {
        public IDBProvider dbProvider { get; }
        public InstrumentsRepository(IDBProvider mdbProvider)
        {
            dbProvider = mdbProvider;
        }


        public ExchangeMarkets GetMarketsFromDb()
        {
            return LoadMarketsFromDB();
        }







        private ExchangeMarkets LoadMarketsFromDB()
        {
            var ds = GetFinamMarketsFromDB();
            ExchangeMarkets allMarketsFromDB = new ExchangeMarkets();
            var dtMarkets = ds.Tables[0];
            var dtInstruments = ds.Tables[1];

            var groupedByMarketId = dtInstruments.Select().GroupBy(r => Convert.ToInt32(r["idMarket"])).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());
            foreach (DataRow oneMarketRow in dtMarkets.Rows)
            {
                var market = Market.FromRow(oneMarketRow);
                var mId = Convert.ToInt32(market.FinamMarketID);
                if (!groupedByMarketId.ContainsKey(mId))
                    continue;

                var instrumentsForMarket = groupedByMarketId[Convert.ToInt32(market.FinamMarketID)];
                foreach (DataRow oneInstrumentRow in instrumentsForMarket)
                {
                    var intstrument = Instrument.FromRow(oneInstrumentRow);
                    market.InstrumentList.Add(intstrument);
                }

                allMarketsFromDB.MarketList.Add(market);
            }
            return allMarketsFromDB;
        }

        public DataSet GetFinamMarketsFromDB()
        {
            var ds = dbProvider.ProcedureDSByName("GetMarketsAndInstruments");
            return ds;
        }


    }
}
