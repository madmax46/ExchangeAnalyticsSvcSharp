using System;
using System.Collections.Generic;
using System.Text;

namespace ExchCommonLib.Classes
{
    public class MarketInstrument
    {
        public int FinamEmitentIDInt { get; set; }
        public string FinamEmitentID { get; set; }
        public string FinamMarketID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParseURL { get; set; }

        public MarketInstrument(string FinamMarketID, string FinamEmitentID, string Code, string Name, string ParseURL)
        {
            this.FinamMarketID = FinamMarketID;
            this.FinamEmitentID = FinamEmitentID;
            this.FinamEmitentIDInt = Convert.ToInt32(FinamEmitentID);
            this.Code = Code;
            this.Name = Name;
            this.ParseURL = ParseURL;
        }

        public static MarketInstrument FromRow(System.Data.DataRow row)
        {
            string finamInstrumentId = Convert.ToString(row["idInstrument"]);
            string finamMarketId = Convert.ToString(row["idMarket"]);
            string name = Convert.ToString(row["instrumentName"]);
            string code = Convert.ToString(row["code"]);
            string parseURL = Convert.ToString(row["parseURL"]);

            MarketInstrument nmi = new MarketInstrument(finamMarketId, finamInstrumentId, code, name, parseURL);

            return nmi;
        }

        public override string ToString()
        {
            return "FinamElementID: " + FinamEmitentID + ", Code: " + Code + ", Name: " + Name + ", ParseURL: " + ParseURL;
        }

    }


}
