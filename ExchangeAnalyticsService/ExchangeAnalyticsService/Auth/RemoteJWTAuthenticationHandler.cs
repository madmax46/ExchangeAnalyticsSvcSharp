using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AuthCommonLib.Authorize;
using ExchangeAnalyticsService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeAnalyticsService.Auth
{
    public class RemoteJWTAuthenticationHandler : AuthenticationHandler<RemoteJWTAuthenticationOptions>
    {
        private const string AuthorizationHeaderName = "Authorization";
        private const string SchemeName = JwtBearerDefaults.AuthenticationScheme;

        private readonly IRemoteJwtAuthenticationService remoteJwtAuthenticationService;

        public RemoteJWTAuthenticationHandler(IOptionsMonitor<RemoteJWTAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IRemoteJwtAuthenticationService authenticationService)
            : base(options, logger, encoder, clock)
        {
            remoteJwtAuthenticationService = authenticationService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(AuthorizationHeaderName))
            {
                //Authorization header not in request
                return AuthenticateResult.NoResult();
            }

            if (!AuthenticationHeaderValue.TryParse(Request.Headers[AuthorizationHeaderName], out AuthenticationHeaderValue headerValue))
            {
                //Invalid Authorization header
                return AuthenticateResult.NoResult();
            }

            if (!SchemeName.Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                //Not authentication header
                return AuthenticateResult.NoResult();
            }

            var tokenRequest = new AuthorizeTokenRequest()
            {
                Token = headerValue.Parameter
            };

            var res = await remoteJwtAuthenticationService.CallCheckServiceExAsync(tokenRequest);

            if (res?.HttpResponse?.StatusCode != System.Net.HttpStatusCode.OK)
                return AuthenticateResult.NoResult();

            var claims = new List<Claim> { };

            if (res.Response?.IsSuccess == true)
            {
                claims.Add(new Claim("UserId", res.Response.User.UserId.ToString()));


                if (!string.IsNullOrEmpty(res.Response.User.FirstName))
                    claims.Add(new Claim("FirstName", res.Response.User.FirstName));

                if (!string.IsNullOrEmpty(res.Response.User.FirstName))
                    claims.Add(new Claim("SecondName", res.Response.User.SecondName));

                if (!string.IsNullOrEmpty(res.Response.User.FirstName))
                    claims.Add(new Claim("Email", res.Response.User.Email));

                var role = res.Response.User.UserRoles.FirstOrDefault();
                if (!string.IsNullOrEmpty(role))
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
            }

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }

}
