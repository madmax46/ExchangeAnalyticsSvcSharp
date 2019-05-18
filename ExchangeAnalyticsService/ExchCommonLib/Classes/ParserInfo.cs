using ExchCommonLib.Classes.Exchange;
using ExchCommonLib.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchCommonLib.Classes
{
    public class ParserInfo
    {
        public int Id { get; set; }
        public DateTime? CurrentParseDate { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public float ProgerssPercent { get; set; }
        public ParseState ParseStatus { get; set; }
        public int IdInstrument { get; set; }
        public int ParsingPerodInDays { get; set; }
        public DateTime StartParseDate { get; set; }
        public DateTime DtUpdate { get; set; }
        public DateTime LastDtTryLoad { get; set; }


        public Instrument Instrument { get; set; }



        public static ParserInfo FromRow(System.Data.DataRow row)
        {
            ParserInfo parserSettings = new ParserInfo();
            try
            {
                parserSettings.Instrument = Instrument.FromRow(row);
            }
            catch (Exception ex)
            {

            }

            int parserId = Convert.ToInt32(row["id"]);
            int parsePeriodInDays = Convert.ToInt32(row["parsePeriodInDays"]);
            DateTime dateStart = Convert.ToDateTime(row["dateStart"]);
            int status = Convert.ToInt32(row["status"]);
            //DateTime dtUpdate = Convert.ToDateTime(row["dtUpdate"]);
            DateTime? lastIsertedDate = !Convert.IsDBNull(row["lastIsertedDate"]) ? Convert.ToDateTime(row["lastIsertedDate"]) : new DateTime?();
            TimeSpan elapsedTime = (TimeSpan)row["elapsedTime"];
            float progressPercent = Convert.ToSingle(row["progressPercent"]);
            DateTime dtUpdate = Convert.ToDateTime(row["dtUpdate"]);

            parserSettings.Id = parserId;
            parserSettings.ParsingPerodInDays = parsePeriodInDays;
            parserSettings.StartParseDate = dateStart;
            parserSettings.ParseStatus = (ParseState)status;
            parserSettings.CurrentParseDate = lastIsertedDate;
            parserSettings.ElapsedTime = elapsedTime;
            parserSettings.ProgerssPercent = progressPercent;
            parserSettings.IdInstrument = parserSettings.Instrument.FinamEmitentIDInt;
            parserSettings.LastDtTryLoad = new DateTime();
            return parserSettings;
        }

    }
}
