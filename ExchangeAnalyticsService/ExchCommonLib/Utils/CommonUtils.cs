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

    }
}
