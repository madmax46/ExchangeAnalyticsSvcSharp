using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Models;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib.Classes;
using ExchCommonLib.Classes.Exchange;
using ExchCommonLib.Classes.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace ExchangeAnalyticsService.Controllers
{

    [Authorize(Roles = "admin, user, readonly")]
    [ApiVersion("1.0")]
    //[Route("api/{version:apiVersion}/[controller]/[action]")]
    [Route("api/v{version:apiVersion}/exchange/[action]")]
    [ApiController]
    public class InstrumentsController : ControllerBase
    {
        private readonly IInstrumentsService instrumentsService;

        public InstrumentsController(IInstrumentsService instrumentsService)
        {
            this.instrumentsService = instrumentsService;
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<ExchangeMarkets>), 200)]
        public ActionResult<ExchangeMarkets> GetMarkets()
        {
            return new ActionResult<ExchangeMarkets>(instrumentsService.GetMarketsFromDb());
        }


        [HttpGet]
        [Authorize(Roles = "admin, user")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<InstrumentsResponse>), 200)]
        public ActionResult<InstrumentsResponse> GetParsedInstruments()
        {
            return instrumentsService.GetParsedInstruments();
        }

    }
}
