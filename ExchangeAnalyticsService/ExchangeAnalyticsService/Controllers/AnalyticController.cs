using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.Models;
using ExchangeAnalyticsService.Models.Responses;
using ExchangeAnalyticsService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeAnalyticsService.Controllers
{
    [Authorize(Roles = "admin, user")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/market/analytics/[action]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            this.analyticsService = analyticsService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IActionResult), 200)]
        public ActionResult<PredictionResponse> GetPredictionFor([FromBody] PredictionRequest predictionsRequest)
        {
            var res = analyticsService.GetPredictionFor(predictionsRequest);
            return res;
        }


    }
}