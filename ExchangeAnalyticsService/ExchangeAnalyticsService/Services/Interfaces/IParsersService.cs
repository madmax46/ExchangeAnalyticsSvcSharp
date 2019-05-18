using ExchangeAnalyticsService.IRepositories;
using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Services.Interfaces
{
    public interface IParsersService
    {
        IParsersRepository ParsersRepository { get; }

        bool RegisterNewParser(ParserInfo parserInfo);

        List<ParserInfo> GetParsers();
    }
}
