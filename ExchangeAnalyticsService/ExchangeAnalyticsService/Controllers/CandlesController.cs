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
using ExchCommonLib.Classes.Responses;

namespace ExchangeAnalyticsService.Controllers
{

    [Authorize(Roles = "admin, user, readonly")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CandlesController : ControllerBase
    {
        private readonly ICandlesService ratesService;

        public CandlesController(ICandlesService ratesService)
        {
            this.ratesService = ratesService;
        }


        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<List<ParserInfo>>), 200)]
        [Route("api/v{version:apiVersion}/market/candles")]
        public ActionResult<CandlesResponse> GetRates([FromBody] CandlesRequest request)
        {
            try
            {
                return new ActionResult<CandlesResponse>(ratesService.GetCandles(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}