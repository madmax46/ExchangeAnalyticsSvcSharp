using System;
using System.Collections.Generic;
using System.Text;

namespace ExchCommonLib.Classes.Responses
{
    public class CandlesResponse
    {
        public List<Candle> Candles { get; set; }
        public Candle LastCandle { get; set; }


        public CandlesResponse()
        {
            Candles = new List<Candle>();
        }
    }
}
