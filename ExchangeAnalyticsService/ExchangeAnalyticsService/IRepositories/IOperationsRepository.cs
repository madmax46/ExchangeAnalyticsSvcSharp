using ExchCommonLib.Classes.Operations;

namespace ExchangeAnalyticsService.IRepositories
{
    public interface IOperationsRepository
    {

        OperationsHistory GetUserOperationsHistory(uint userId);

        bool SaveUserOperationToDb(uint userId, MarketOperation operation);

        bool DeleteUserOperation(uint userId, uint operationId);
    }
}
