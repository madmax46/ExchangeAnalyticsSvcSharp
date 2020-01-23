using ExchCommonLib.Classes;
using ExchCommonLib.Enums;
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

        public static CandlesInterval ParseCandlesIntervaFromStr(string interval)
        {
            switch (interval)
            {
                case "1min": return CandlesInterval.Min;
                case "5min": return CandlesInterval.FiveMin;
                case "15min": return CandlesInterval.FifteenMin;
                case "30min": return CandlesInterval.ThirtyMin;
                case "hour": return CandlesInterval.Hour;
                case "day": return CandlesInterval.Day;
                case "week": return CandlesInterval.Week;
                case "month": return CandlesInterval.Month;
                default: return CandlesInterval.Min;
            }
        }


    }
}
