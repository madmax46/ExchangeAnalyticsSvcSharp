using ExchCommonLib.Classes.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Models
{
    public class InstrumentsResponse
    {
        public List<Instrument> Instruments { get; set; }

        public InstrumentsResponse()
        {
            Instruments = new List<Instrument>();
        }
    }
}
