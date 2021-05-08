using ActivityTrackerApi.Data.DTOs.Activities;
using ActivityTrackerApi.Data.Models;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Clients
{
    public class StravaClient : IStravaClient
    {
        const string BaseUrl = "https://www.strava.com/api/v3";

        private readonly IRestClient _restClient;

        public StravaClient()
        {
            _restClient = new RestClient(BaseUrl);
        }
        public IRestClient RestClient => _restClient;

        public async Task<IEnumerable<StravaActivityDto>> GetActivities(string token)
        {
            var request = new RestRequest("activities", DataFormat.Json);
            request.AddHeader("authorization", "Bearer " + token);
            //var activities = await RestClient.GetAsync<IEnumerable<StravaActivityDto>>(request);

            var response = await RestClient.ExecuteAsync<IEnumerable<StravaActivityDto>>(request);
            if (!response.IsSuccessful)
                throw new Exception("TEST");

            return response.Data;

        }
    }
}
