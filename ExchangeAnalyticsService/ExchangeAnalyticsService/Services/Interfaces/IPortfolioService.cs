using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchCommonLib.Classes.Operations;
using ExchCommonLib.Classes.UserPortfolio;

namespace ExchangeAnalyticsService.Services.Interfaces
{
    public interface IPortfolioService
    {
        Portfolio LoadUserPortfolio(uint userId);
        OperationsHistory GetUserOperationsHistory(uint userId);

        bool SaveUserOperationToDb(uint userId, MarketOperation operation);

        bool DeleteUserOperation(uint userId, uint operationId);
    }
}
