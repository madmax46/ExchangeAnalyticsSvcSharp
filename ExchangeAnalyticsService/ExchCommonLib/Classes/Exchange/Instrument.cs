using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace ExchCommonLib.Classes.Exchange
{

    public class Instrument
    {
        [JsonProperty(PropertyName = "FEmID")]
        public int FinamEmitentIDInt { get; set; } //Это idInstrument
        [JsonProperty(PropertyName = "FEmIDs")]
        public string FinamEmitentID { get; set; }
        [JsonProperty(PropertyName = "FMID")]
        public string FinamMarketID { get; set; }
        [JsonProperty(PropertyName = "C")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "URL")]
        public string ParseURL { get; set; }

        public Instrument(string FinamMarketID, string FinamEmitentID, string Code, string Name, string ParseURL)
        {
            this.FinamMarketID = FinamMarketID;
            this.FinamEmitentID = FinamEmitentID;
            this.FinamEmitentIDInt = Convert.ToInt32(FinamEmitentID);
            this.Code = Code;
            this.Name = Name;
            this.ParseURL = ParseURL;
        }

        public static Instrument FromRow(System.Data.DataRow row)
        {
            string finamInstrumentId = Convert.ToString(row["idInstrument"]);
            string finamMarketId = Convert.ToString(row["idMarket"]);
            string name = Convert.ToString(row["instrumentName"]);
            string code = Convert.ToString(row["code"]);
            string parseURL = Convert.ToString(row["parseURL"]);

            Instrument nmi = new Instrument(finamMarketId, finamInstrumentId, code, name, parseURL);

            return nmi;
        }

        public override string ToString()
        {
            return "FinamElementID: " + FinamEmitentID + ", Code: " + Code + ", Name: " + Name + ", ParseURL: " + ParseURL;
        }

    }


}
