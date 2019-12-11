using System;
using System.Collections.Generic;
using System.Text;

namespace ExchCommonLib.Classes.Requests
{
    public class CandlesRequest
    {
        public uint InstrumentId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Interval { get; set; }

        public List<string> Indicators { get; set; }

        public CandlesRequest()
        {
            Indicators = new List<string>();
        }
    }
}
