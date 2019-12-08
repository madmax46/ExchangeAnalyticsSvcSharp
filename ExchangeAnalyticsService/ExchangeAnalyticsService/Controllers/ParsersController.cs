using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeAnalyticsService.Controllers
{
    [Authorize]
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


        [Authorize(Roles = "admin, user")]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<bool>), 200)]
        public ActionResult<bool> RegisterNewParser([FromBody] ParserInfo parserInfo)
        {
            return ParsersService.RegisterNewParser(parserInfo);
        }

        [Authorize(Roles = "admin, user, readonly")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<List<ParserInfo>>), 200)]
        public ActionResult<List<ParserInfo>> GetParsers()
        {
            return ParsersService.GetParsers();
        }

    }
}