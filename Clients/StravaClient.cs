using ActivityTrackerApi.Data.DTOs.StravaAuth;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Clients
{
    public class StravaClient : IStravaClient
    {
        const string BaseUrl = "https://www.strava.com/oauth";

        private readonly IRestClient _restClient;
        public IRestClient RestClient => _restClient;

        public StravaClient()
        {
            _restClient = new RestClient(BaseUrl);
        }

        public async Task<object> GetToken()
        {
            string client_id = "63072";
            string client_secret = "563a513fe062b03fa26ad2e9814a5d906826b3be";
            string code = "390b90c574f5b47fb4c52c0fd6ce7b02e5d77ee9";
            string grant_type = "authorization_code";
            var request = new RestRequest("token", Method.POST);

            var test = new TokenResponse();

            request.AddObject(test);
            request.AddParameter("client_id", client_id);
            request.AddParameter("client_secret", client_secret);
            request.AddParameter("code", code);
            request.AddParameter("grant_type", grant_type);

            var content = await RestClient.ExecuteAsync<TokenResponse>(request);
            

            var data = content.Data;

            return content;
        }
    }
}
