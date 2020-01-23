using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbWrapperCore;
using ExchangeAnalyticsService.DbHandlers;
using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Models.Analytic;
using Microsoft.Extensions.Logging;

namespace ExchangeAnalyticsService.Repositories
{
    public class TechMethodsRepository : ITechMethodsRepository
    {
        private readonly ILogger<TechMethodsRepository> logger;
        private readonly IDBProvider dbProvider; //база smanalytics

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbProvider"> база smanalytics</param>
        /// <param name="logger"></param>
        public TechMethodsRepository(IDBProvider dbProvider, ILogger<TechMethodsRepository> logger)
        {
            this.dbProvider = dbProvider;
            this.logger = logger;
        }

        public List<AnalyticTechMethodsDbInfo> GetTechMethodsInfoFromDb()
        {
            try
            {
                var infos = dbProvider.Query("svc_getTechMethodsInfo", new AnTechMethodDbHandler());
                return infos;
            }
            catch (Exception e)
            {
                logger.LogError(e, "error");
                return null;
            }
        }
    }
}
