using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchCommonLib.Classes.Exchange
{
    public class Market
    {
        [JsonProperty(PropertyName = "MID")]
        public string FinamMarketID { get; set; }
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Items")]
        public List<Instrument> InstrumentList { get; set; }
        public Market()
        {
            InstrumentList = new List<Instrument>();
        }
        public Market(string FinamMarketID, string Name)
        {
            this.FinamMarketID = FinamMarketID;
            this.Name = Name;
            InstrumentList = new List<Instrument>();
        }
        public override string ToString()
        {
            return "FinamElementID: " + FinamMarketID + ", Name: " + Name;
        }


        public static Market FromRow(System.Data.DataRow row)
        {
            string finamMarketId = Convert.ToString(row["id"]);
            string name = Convert.ToString(row["name"]);
            Market nm = new Market(finamMarketId, name);

            return nm;
        }

    }

}
