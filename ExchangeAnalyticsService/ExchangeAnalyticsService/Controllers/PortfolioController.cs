using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib.Classes.Operations;
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
    //[Route("api/v{version:apiVersion}/portfolio/[action]")]
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
        [Route("api/v{version:apiVersion}/portfolio/get")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<Portfolio>), 200)]
        public Portfolio GetPortfolio()
        {
            var userIdConv = GetUserId();
            return portfolioService.LoadUserPortfolio(userIdConv);
        }


        [HttpPost]
        [Authorize(Roles = "admin, user")]
        [Route("api/v{version:apiVersion}/portfolio/operations/save")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<bool>), 200)]
        public ActionResult<bool> SaveOperation([FromBody] MarketOperation marketOperation)
        {
            var userIdConv = GetUserId();
            var res = portfolioService.SaveUserOperationToDb(userIdConv, marketOperation);
            //return portfolioService.LoadUserPortfolio(1);
            return res;
        }

        private uint GetUserId()
        {
            var userId = User.Claims.FirstOrDefault(r => r.Type == "UserId");
            var userIdConv = userId != null ? Convert.ToUInt32(userId.Value) : 0;
            return userIdConv;
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        [Route("api/v{version:apiVersion}/portfolio/operations/history")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<OperationsHistory>), 200)]
        public ActionResult<OperationsHistory> LoadHistoryOperation()
        {
            var userIdConv = GetUserId();
            var res = portfolioService.GetUserOperationsHistory(userIdConv);
            return res;
        }


        [HttpGet]
        [Produces("application/json")]
        [Route("api/v{version:apiVersion}/portfolio/operations/delete")]
        [Authorize(Roles = "admin, user")]
        [ProducesResponseType(typeof(ActionResult<bool>), 200)]
        public ActionResult<bool> RemoveOperation(uint operationId)
        {
            var userIdConv = GetUserId();
            var res = portfolioService.DeleteUserOperation(userIdConv, operationId);
            return res;
        }
    }
}