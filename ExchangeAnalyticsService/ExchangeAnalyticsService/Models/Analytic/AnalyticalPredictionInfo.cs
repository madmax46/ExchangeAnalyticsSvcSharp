using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Models.Analytic
{
    public class AnalyticalPredictionInfo
    {

        public string MethodName { get; set; }
        public uint IndicatorType { get; set; }

        public double? Value { get; set; }


        public string PredictionDecision { get; set; }

        public AnalyticalPredictionInfo(string methodName, double? value, string predictionDecision, uint indicatorType)
        {
            MethodName = methodName;
            Value = value;
            PredictionDecision = predictionDecision;
            IndicatorType = indicatorType;
        }
    }
}
