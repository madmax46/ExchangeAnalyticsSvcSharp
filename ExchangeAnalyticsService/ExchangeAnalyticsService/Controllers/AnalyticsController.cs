using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeAnalyticsService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/exchange/[action]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {



    }
}