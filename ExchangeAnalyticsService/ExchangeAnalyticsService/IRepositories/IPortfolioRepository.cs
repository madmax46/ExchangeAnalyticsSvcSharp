using ExchCommonLib.Classes.UserPortfolio;

namespace ExchangeAnalyticsService.IRepositories
{
    public interface IPortfolioRepository
    {

        Portfolio LoadUserPortfolio(uint userId);
    }
}
