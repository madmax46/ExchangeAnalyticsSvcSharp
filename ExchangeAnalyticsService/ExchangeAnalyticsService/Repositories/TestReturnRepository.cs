using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Models;
using ExchangeAnalyticsService.Utils;
using ExchCommonLib;
using ExchCommonLib.Classes;
using ExchCommonLib.Classes.Requests;
using ExchCommonLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Repositories
{
    public class TestReturnRepository : ITestReturnRepository
    {


        List<TestReturnData> testReturnDatas = new List<TestReturnData>();

        public void Add(TestReturnData item)
        {
            testReturnDatas.Add(item);
        }

        public TestReturnData Find(string key)
        {
            return testReturnDatas.FirstOrDefault(r => r.Key == key);
        }

        public IEnumerable<TestReturnData> GetAll()
        {
            return testReturnDatas;
        }

        public IEnumerable<ParserInfo> GetAllPrsers()
        {
            var table = CommonDbCalls.GetAllParsersFromDB(DbUtils.MariaDbWrapper);
            var retVal = CommonUtils.GetParserInfosFromDt(table);
            return retVal;
        }

        public IEnumerable<Rate> GetPeriodQuotesByInstrument(InstrumentPricesRequest request)
        {
            throw new NotImplementedException();
        }

        public TestReturnData Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void Update(TestReturnData item)
        {
            throw new NotImplementedException();
        }
    }
}
