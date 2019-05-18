using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib.Classes;
using ExchCommonLib.Classes.Exchange;
using ExchCommonLib.Classes.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace ExchangeAnalyticsService.Controllers
{

    [ApiVersion("1.0")]
    //[Route("api/{version:apiVersion}/[controller]/[action]")]
    [Route("api/v{version:apiVersion}/exchange/[action]")]
    [ApiController]
    public class InstrumentsController : ControllerBase
    {
        private IInstrumentsService InstrumentsService { get; set; }

        public InstrumentsController(IInstrumentsService instrumentsService)
        {
            InstrumentsService = instrumentsService;
        }


        [HttpGet]
        public ActionResult<ExchangeMarkets> GetMarkets()
        {
            return new ActionResult<ExchangeMarkets>(InstrumentsService.GetMarketsFromDb());
        }


    }
}
