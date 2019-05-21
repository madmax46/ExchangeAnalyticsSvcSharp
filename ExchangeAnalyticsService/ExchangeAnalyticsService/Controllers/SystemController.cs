using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeAnalyticsService.Controllers
{
    [ApiVersion("1.0")]
    [Route("[action]")]
    public class SystemController : ControllerBase
    {
        [HttpGet]
        public ActionResult Unauthorized()
        {
            return StatusCode(401, "{\"message\":\"Неавторизованный вход\"}");
        }

        [HttpGet]
        public ActionResult Forbidden()
        {
            return StatusCode(403, "{\"message\":\"Действие запрещено\"}");
        }
    }
}