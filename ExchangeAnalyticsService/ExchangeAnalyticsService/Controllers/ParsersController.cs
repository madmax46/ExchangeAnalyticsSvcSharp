using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeAnalyticsService.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/exchange/[action]")]
    [ApiController]
    public class ParsersController : ControllerBase
    {
        private IParsersService ParsersService { get; }

        public ParsersController(IParsersService parsersService)
        {
            ParsersService = parsersService;
        }



        [HttpPost]
        public ActionResult<bool> RegisterNewParser([FromBody] ParserInfo parserInfo)
        {
            return ParsersService.RegisterNewParser(parserInfo);
        }


        [HttpGet]
        public ActionResult<List<ParserInfo>> GetParsers()
        {
            return ParsersService.GetParsers();
        }

    }
}