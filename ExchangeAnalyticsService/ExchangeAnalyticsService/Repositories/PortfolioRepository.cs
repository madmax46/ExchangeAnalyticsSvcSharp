using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DbWrapperCore;
using ExchangeAnalyticsService.IRepositories;
using ExchCommonLib.Classes.UserPortfolio;
using Microsoft.Extensions.Logging;

namespace ExchangeAnalyticsService.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ILogger<PortfolioRepository> logger;
        private readonly IDBProvider dbProvider;

        public PortfolioRepository(ILogger<PortfolioRepository> logger, IDBProvider dbProvider)
        {
            this.logger = logger;
            this.dbProvider = dbProvider;
        }

        public Portfolio LoadUserPortfolio(uint userId)
        {

            var portfolio = new Portfolio();
            try
            {

                var table = dbProvider.ProcedureByName("svc_GetUserPortfolio", userId);
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        portfolio.Positions.Add(new PortfolioItem()
                        {
                            InstrumentId = Convert.ToUInt32(row["instrumentId"]),
                            //CurСost =  Convert.ToUInt32(row["balance"]),
                            RemCount = Convert.ToInt32(row["remCount"]),
                            AvgPrice = Convert.ToDouble(row["avgPrice"]),
                        });
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "");
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "");
            }
            return portfolio;
        }
    }
}
