using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeAnalyticsService.Controllers
{
    [Authorize(Roles = "admin, user")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/exchange/[action]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IActionResult), 200)]
        public IActionResult GetPredictionFor([FromBody] PredictionRequest predictionsRequest)
        {

            return StatusCode(200);
        }


    }
}