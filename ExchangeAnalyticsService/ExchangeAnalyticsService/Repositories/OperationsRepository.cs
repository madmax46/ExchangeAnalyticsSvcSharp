using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DbWrapperCore;
using ExchangeAnalyticsService.IRepositories;
using ExchCommonLib.Classes.Operations;
using ExchCommonLib.Enums;
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
            var historyTable = dbProvider.ProcedureByName("svc_getOperationsHistory", userId, 1000);
            var history = new OperationsHistory();
            foreach (DataRow oneRow in historyTable.Rows)
            {
                history.Operations.Add(new MarketOperation()
                {
                    Id = Convert.ToUInt32(oneRow["id"]),
                    InstrumentId = Convert.ToUInt32(oneRow["instrumentId"]),
                    Date = Convert.ToDateTime(oneRow["datetime"]),
                    OrderType = (OperationType)Convert.ToInt32(oneRow["orderType"]),
                    Price = Convert.ToDouble(oneRow["price"]),
                    Count = Convert.ToInt32(oneRow["volume"]),
                });
            }

            return history;
        }

        public bool SaveUserOperationToDb(uint userId, MarketOperation operation)
        {
            dbProvider.ProcedureByName("svc_saveUserOperation", userId, operation.InstrumentId, operation.Count, operation.Price, operation.OrderType, operation.Date);
            return true;
        }

        public bool DeleteUserOperation(uint userId, uint operationId)
        {
            dbProvider.ProcedureByName("svc_userDeleteOperation", userId, operationId);
            return true;
        }
    }
}
