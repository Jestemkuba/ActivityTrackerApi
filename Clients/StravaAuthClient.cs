using ActivityTrackerApi.Data.DTOs.StravaAuth;
using RestSharp;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace ActivityTrackerApi.Clients
{
    public class StravaAuthClient : IStravaAuthClient
    {
        const string BaseUrl = "https://www.strava.com/oauth";

        private readonly IConfiguration _configuration;
        private readonly IRestClient _restClient;
        public IRestClient RestClient => _restClient;

        public StravaAuthClient(IConfiguration configuration)
        {
            _restClient = new RestClient(BaseUrl);
            _configuration = configuration;
        }

        public async Task<TokenResponse> GetAuthToken(string code)
        {
            var request = new RestRequest("token", Method.POST);

            request.AddParameter("client_id", _configuration["Strava:Id"]);
            request.AddParameter("client_secret", _configuration["Strava:Secret"]);
            request.AddParameter("code", code);
            request.AddParameter("grant_type", "authorization_code");

            var tokenResponse = await RestClient.ExecuteAsync<TokenResponse>(request);
            return tokenResponse.Data;
        }
    }
}
