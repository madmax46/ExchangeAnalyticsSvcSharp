using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Models;
using ExchCommonLib.Classes;
using ExchCommonLib.Classes.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace ExchangeAnalyticsService.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    //[Route("api/{version:apiVersion}/[controller]/[action]")]
    [Route("api/v{version:apiVersion}/exchange/[action]")]
    [ApiController]
    public class MainExchangeController : ControllerBase
    {

        public MainExchangeController(ITestReturnRepository repository)
        {
            TestRepository = repository;
        }
        public ITestReturnRepository TestRepository { get; set; }


        [HttpGet]
        public ActionResult<IEnumerable<ParserInfo>> GetAllParsers()
        {
            return new ActionResult<IEnumerable<ParserInfo>>(TestRepository.GetAllPrsers());
        }

        [HttpGet]
        [HttpPost]
        public ActionResult<bool> RegisterNewParser([FromBody] ParserInfo parserInfo)
        {
            return false;
        }

        [HttpGet]
        public string Hello()
        {
            return "Дратути :)";
        }

        [HttpGet]
        public ActionResult<IEnumerable<TestReturnData>> GetAllTestData()
        {
            return new ActionResult<IEnumerable<TestReturnData>>(TestRepository.GetAll());
        }


        //[HttpGet]
        [HttpPost]
        public ActionResult<IEnumerable<Rate>> GetPeriodQuotesByInstrument([FromBody] InstrumentPricesRequest request)
        {
            return new ActionResult<IEnumerable<Rate>>(TestRepository.GetPeriodQuotesByInstrument(request));
        }



        //public ActionResult<bool> InsertNewTestData([FromBody] TestReturnData data)
        //{
        //    TestRepository.Add(data);
        //    return true;
        //}
    }
}
