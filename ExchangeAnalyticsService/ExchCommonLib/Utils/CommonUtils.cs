using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ExchCommonLib.Utils
{
    public static class CommonUtils
    {

        public static IEnumerable<ParserInfo> GetParserInfosFromDt(DataTable dataTable)
        {
            List<ParserInfo> parsers = new List<ParserInfo>();

            foreach (DataRow oneRow in dataTable.Rows)
            {
                parsers.Add(ParserInfo.FromRow(oneRow));
            }
            return parsers;
        }

        public static void RegisterNewParser(IDBProvider dbProvider, ParserInfo parserSettingsDb)
        {
            var dtId = dbProvider.ProcedureByName("registerNewParser",
                            dbProvider.ToSqlParam(parserSettingsDb.IdInstrument),
                            dbProvider.ToSqlParam(parserSettingsDb.ParsingPerodInDays),
                            dbProvider.ToSqlParam(parserSettingsDb.StartParseDate),
                            dbProvider.ToSqlParam(parserSettingsDb.ParseStatus));

            int id = Convert.ToInt32(dtId.Rows[0][0]);
            parserSettingsDb.Id = id;
        }
    }
}
