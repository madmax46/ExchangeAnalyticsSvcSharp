using ExchCommonLib;
using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.IRepositories
{
    public interface IParsersRepository
    {
        IDBProvider dbProvider { get; }

        List<ParserInfo> GetAll();
        void AddNew(ParserInfo parserInfo);

    }
}
