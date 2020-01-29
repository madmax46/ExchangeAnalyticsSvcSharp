using System;
using System.Collections.Generic;
using System.Text;

namespace ExchCommonLib.Classes.Requests
{
    public class LastCandleRequest
    {

        public uint InstrumentId { get; set; }
        public string Interval { get; set; }

        public LastCandleRequest()
        {

        }
        public LastCandleRequest(uint instrumentId, string interval)
        {
            InstrumentId = instrumentId;
            Interval = interval;
        }

    }
}
