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
using Microsoft.AspNetCore.Authorization;

namespace ExchangeAnalyticsService.Controllers
{

    [Authorize(Roles = "admin, user, readonly")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/exchange/[action]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private IRatesService RatesService { get; set; }

        public RatesController(IRatesService ratesService)
        {
            RatesService = ratesService;
        }


        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<List<ParserInfo>>), 200)]
        public ActionResult<List<Rate>> GetRates(uint instrumentId, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                return new ActionResult<List<Rate>>(RatesService.GetRatesFromDb(instrumentId, dateStart, dateEnd));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}