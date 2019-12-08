using AuthCommonLib.Authorize;
using ExchangeAnalyticsService.Auth.Classes;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchCommonLib.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Services
{
    public class RemoteJwtAuthenticationService : IRemoteJwtAuthenticationService
    {
        private readonly HttpClient httpClient;
        public string TokenServiceAddress { get; private set; }
        public RemoteJwtAuthenticationService(string tokenServiceAddress)
        {
            TokenServiceAddress = tokenServiceAddress;
            httpClient = new HttpClient();
            ConfigurateHttpClient();
        }

        private void ConfigurateHttpClient()
        {
            ServicePointManager.DefaultConnectionLimit = 5;
            httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
        }


        public async Task<CallResponse<AuthorizeTokenResponse>> CallCheckServiceExAsync(AuthorizeTokenRequest req)
        {
            return await CallRest.PostAsync<AuthorizeTokenRequest, AuthorizeTokenResponse>(this.TokenServiceAddress, req, httpClient);
        }
    }

}
