using AuthCommonLib.Authorize;
using ExchangeAnalyticsService.Auth.Classes;
using ExchCommonLib.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Services.Interfaces
{
    public interface IRemoteJwtAuthenticationService
    {
        string TokenServiceAddress { get; }

        Task<CallResponse<AuthorizeTokenResponse>> CallCheckServiceExAsync(AuthorizeTokenRequest req);

    }
}
