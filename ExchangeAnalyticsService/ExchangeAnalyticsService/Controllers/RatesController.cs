using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Models;
using ExchCommonLib.Classes;
using ExchCommonLib.Classes.Exchange;
using ExchCommonLib.Classes.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace ExchangeAnalyticsService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/exchange/[action]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private IRatesRepository RatesRepository { get; set; }

        public RatesController(IRatesRepository ratesRepository)
        {
            RatesRepository = ratesRepository;
        }



        [HttpGet]
        public ActionResult<List<Rate>> GetRates(uint instrumentId, DateTime dateStart, DateTime dateEnd)
        {
            return new ActionResult<List<Rate>>(RatesRepository.GetRatesFromDb(instrumentId, dateStart, dateEnd));
        }
    }
}