using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.Models;
using ExchangeAnalyticsService.Models.Responses;

namespace ExchangeAnalyticsService.Services.Interfaces
{
    public interface IAnalyticsService
    {
        PredictionResponse GetPredictionFor(PredictionRequest predictionsRequest);


    }
}
