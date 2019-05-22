using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Models
{
    public class PredictionRequest
    {
        public uint InstrumentId { get; set; }
        public DateTime PredictionTime { get; set; }
    }
}
