using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Services
{
    public class ParsersService : IParsersService
    {
        public IParsersRepository ParsersRepository { get; private set; }

        public ParsersService(IParsersRepository parsersRepository)
        {
            ParsersRepository = parsersRepository;
        }


        public bool RegisterNewParser(ParserInfo parserInfo)
        {
            try
            {
                ParsersRepository.AddNew(parserInfo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<ParserInfo> GetParsers()
        {
            List<ParserInfo> parserInfos = new List<ParserInfo>();
            try
            {
                parserInfos = ParsersRepository.GetAll();
            }
            catch
            {

            }
            return parserInfos;
        }
    }
}
