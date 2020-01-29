using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbWrapperCore;
using ExchangeAnalyticsService.IRepositories;
using ExchCommonLib.Classes.Operations;
using Microsoft.Extensions.Logging;

namespace ExchangeAnalyticsService.Repositories
{
    public class OperationsRepository : IOperationsRepository
    {

        private readonly ILogger<OperationsRepository> logger;
        private readonly IDBProvider dbProvider;

        public OperationsRepository(ILogger<OperationsRepository> logger, IDBProvider dbProvider)
        {
            this.dbProvider = dbProvider;
            this.logger = logger;
        }

        public OperationsHistory GetUserOperationsHistory(uint userId)
        {
            throw new NotImplementedException();
        }

        public bool SaveUserOperationToDb(uint userId, MarketOperation operation)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUserOperation(uint userId, uint operationId)
        {
            throw new NotImplementedException();
        }
    }
}
