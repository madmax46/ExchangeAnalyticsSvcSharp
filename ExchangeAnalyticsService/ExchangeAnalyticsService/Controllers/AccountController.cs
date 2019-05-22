using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using System.Security.Claims;
using ExchangeAnalyticsService.Models;
using ExchangeAnalyticsService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ExchangeAnalyticsService.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService AccountService { get; set; }


        public AccountController(IAccountService accountService)
        {
            AccountService = accountService;
        }



        //public async Task Login([FromBody] LoginInfo loginInfo)
        //{

        //    var username = loginInfo.Username;
        //    var password = loginInfo.Password;

        //    var identity = GetIdentity(username, password);
        //    if (identity == null)
        //    {
        //        Response.StatusCode = 400;
        //        await Response.WriteAsync("Invalid username or password.");
        //        return;
        //    }

        //    var now = DateTime.UtcNow;
        //    // создаем JWT-токен
        //    var jwt = new JwtSecurityToken(
        //            issuer: AuthOptions.ISSUER,
        //            audience: AuthOptions.AUDIENCE,
        //            notBefore: now,
        //            claims: identity.Claims,
        //            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
        //            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        //    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        //    var response = new
        //    {
        //        access_token = encodedJwt,
        //        username = identity.Name
        //    };

        //    // сериализация ответа
        //    Response.ContentType = "application/json";
        //    await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        //}
        [HttpPost("/login")]
        public async Task Login([FromBody] LoginInfo loginInfo)
        {
            try
            {
                var loginResult = AccountService.Login(loginInfo);

                if (loginResult == null)
                {
                    Response.StatusCode = 400;
                    await Response.WriteAsync("Invalid username or password.");
                    return;
                }

                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonConvert.SerializeObject(loginResult, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            }
            catch
            {
                Response.StatusCode = 500;
                await Response.WriteAsync("Error.");
            }



            //var username = loginInfo.Username;
            //var password = loginInfo.Password;

            //var identity = GetIdentity(username, password);
            //if (identity == null)
            //{
            //    Response.StatusCode = 400;
            //    await Response.WriteAsync("Invalid username or password.");
            //    return;
            //}




            //var now = DateTime.UtcNow;
            //// создаем JWT-токен
            //var jwt = new JwtSecurityToken(
            //        issuer: AuthOptions.ISSUER,
            //        audience: AuthOptions.AUDIENCE,
            //        notBefore: now,
            //        claims: identity.Claims,
            //        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            //        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            //var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //var response = new
            //{
            //    access_token = encodedJwt,
            //    username = identity.Name
            //};

            //// сериализация ответа
            //Response.ContentType = "application/json";
            //await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }


        [Authorize]
        [Route("/getname")]
        public IActionResult GetName()
        {
            try
            {
                return Ok($"Ваш логин: {User.Identity.Name}");
            }
            catch
            {
                return StatusCode(500);
            }
        }

    }
}
