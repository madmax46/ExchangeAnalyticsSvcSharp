using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.Models;
using ExchCommonLib.Classes.Requests;
using ExchCommonLib.Classes.Responses;

namespace ExchangeAnalyticsService.Services.Interfaces
{
    public interface IAnalyticsService
    {
        PredictionResponse GetPredictionFor(PredictionRequest predictionsRequest);


    }
}
