﻿using System;
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
            var claims = User.Claims;
            return portfolioService.LoadUserPortfolio(1);
        }


        [HttpPost]
        [Authorize(Roles = "admin, user")]
        [Route("api/v{version:apiVersion}/portfolio/operations/save")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<bool>), 200)]
        public ActionResult<bool> SaveOperation([FromBody] MarketOperation marketOperation)
        {
            var claims = User.Claims;
            var res = portfolioService.SaveUserOperationToDb(1, marketOperation);
            //return portfolioService.LoadUserPortfolio(1);
            return res;
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        [Route("api/v{version:apiVersion}/portfolio/operations/history")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult<OperationsHistory>), 200)]
        public ActionResult<OperationsHistory> LoadHistoryOperation()
        {
            var claims = User.Claims;
            var res = portfolioService.GetUserOperationsHistory(1);
            return res;
        }


        [HttpGet]
        [Produces("application/json")]
        [Route("api/v{version:apiVersion}/portfolio/operations/delete")]
        [Authorize(Roles = "admin, user")]
        [ProducesResponseType(typeof(ActionResult<bool>), 200)]
        public ActionResult<bool> RemoveOperation(uint operationId)
        {
            var claims = User.Claims;
            var res = portfolioService.DeleteUserOperation(1, operationId);
            return res;
        }
    }
}