using DbWrapperCore;
using ExchangeAnalyticsService.IRepositories;
using ExchCommonLib;
using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Repositories
{
    public class ParsersRepository : IParsersRepository
    {

        public IDBProvider dbProvider { get; private set; }


        public ParsersRepository(IDBProvider dBProvider)
        {
            this.dbProvider = dBProvider;
        }

        public List<ParserInfo> GetAll()
        {
            var table = dbProvider.ProcedureByName("svc_getParsers");

            List<ParserInfo> parsersInfo = new List<ParserInfo>();
            foreach (DataRow oneRow in table.Rows)
            {
                var parser = ParserInfo.FromRow(oneRow);
                parsersInfo.Add(parser);
            }
            return parsersInfo;
        }

        public void AddNew(ParserInfo parserInfo)
        {
            var dtId = dbProvider.ProcedureByName("registerNewParser",
                            dbProvider.ToSqlParam(parserInfo.IdInstrument),
                            dbProvider.ToSqlParam(parserInfo.ParsingPerodInDays),
                            dbProvider.ToSqlParam(parserInfo.StartParseDate),
                            dbProvider.ToSqlParam(parserInfo.ParseStatus));
        }

    }
}
