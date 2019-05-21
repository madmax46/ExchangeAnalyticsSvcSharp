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
using ExchangeAnalyticsService.IRepositories;

namespace ExchangeAnalyticsService.Services
{
    public class AccountService : IAccountService
    {
        IAccountRepository AccountRepository { get; }


        public AccountService(IAccountRepository accountRepository)
        {
            AccountRepository = accountRepository;
        }

        //private List<Person> people = new List<Person>
        //{
        //    new Person {Login="admin@gmail.com", Password="12345", Role = "admin" },
        //    new Person { Login="qwerty", Password="55555", Role = "user" }
        //};

        public AuthenticateResponse Login(LoginInfo loginInfo)
        {
            var username = loginInfo.Username;
            var password = loginInfo.Password;

            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


            return new AuthenticateResponse(encodedJwt, identity.Name);
        }


        private ClaimsIdentity GetIdentity(string username, string password)
        {
            Person person = AccountRepository.GetUserByLoginAndPassword(username, password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
