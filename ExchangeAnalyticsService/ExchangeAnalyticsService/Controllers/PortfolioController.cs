using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib.Classes.UserPortfolio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExchangeAnalyticsService.Controllers
{
    [Authorize(Roles = "admin, user")]
    [ApiVersion("1.0")]
    //[Route("api/{version:apiVersion}/[controller]/[action]")]
    [Route("api/v{version:apiVersion}/portfolio/[action]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {

        private readonly ILogger<PortfolioController> logger;
        private readonly IPortfolioService portfolioService;

        public PortfolioController(ILogger<PortfolioController> logger, IPortfolioService portfolioService)
        {
            this.logger = logger;
            this.portfolioService = portfolioService;
        }


        [HttpGet]
        [Authorize(Roles = "admin, user")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<Portfolio>), 200)]
        public Portfolio GetPortfolio()
        {
            var claims = User.Claims;
            return portfolioService.LoadUserPortfolio(1);
        }
    }
}