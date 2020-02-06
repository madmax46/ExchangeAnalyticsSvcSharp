using System;
using System.Collections.Generic;
using System.Text;
using TechAnalysisAlgLib.Interfaces;

namespace ExchCommonLib.Classes.Responses
{
    public class CandlesResponse
    {
        public List<Candle> Candles { get; set; }
        public Candle LastCandle { get; set; }


        public List<IndicatorResponse> IndicatorResponses { get; set; }


        public CandlesResponse()
        {
            Candles = new List<Candle>();
        }
    }

    public class IndicatorResponse
    {
        public string IndicatorName { get; set; }

        public ISeries SeriesData { get; set; }
    }

}
