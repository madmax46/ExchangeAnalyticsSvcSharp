using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.Models.Analytic;

namespace ExchangeAnalyticsService.IRepositories
{
    public interface ITechMethodsRepository
    {
        List<AnalyticTechMethodsDbInfo> GetTechMethodsInfoFromDb();
    }
}
