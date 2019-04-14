using ExchangeAnalyticsService.Models;
using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.IRepositories
{
    public interface ITestReturnRepository
    {
        void Add(TestReturnData item);
        IEnumerable<TestReturnData> GetAll();

        IEnumerable<ParserInfo> GetAllPrsers();

        TestReturnData Find(string key);
        TestReturnData Remove(string key);
        void Update(TestReturnData item);
    }
}
