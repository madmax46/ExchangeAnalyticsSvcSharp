using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.Analytic;
using ExchangeAnalyticsService.Models;
using ExchangeAnalyticsService.Models.Responses;
using ExchangeAnalyticsService.Services.Interfaces;

namespace ExchangeAnalyticsService.Services
{
    public class AnalyticsService : IAnalyticsService
    {

        private readonly InstrumentsTechAnalyser instrumentsTechAnalyser;
        public AnalyticsService(InstrumentsTechAnalyser instrumentsTechAnalyser)
        {

            this.instrumentsTechAnalyser = instrumentsTechAnalyser;
        }
        //AnalyseInstrument
        public PredictionResponse GetPredictionFor(PredictionRequest predictionsRequest)
        {
            var predictResult = instrumentsTechAnalyser.AnalyseInstrument(predictionsRequest.InstrumentId, predictionsRequest.Interval);

            return new PredictionResponse() { Predictions = predictResult };
        }
    }
}

