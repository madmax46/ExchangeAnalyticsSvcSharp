using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.Models.Analytic;

namespace ExchangeAnalyticsService.Models.Responses
{
    public class PredictionResponse
    {
        public List<AnalyticalPredictionInfo> Predictions { get; set; }
    }
}
