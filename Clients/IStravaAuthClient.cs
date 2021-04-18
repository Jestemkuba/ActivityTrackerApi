using ActivityTrackerApi.Data.DTOs.StravaAuth;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Clients
{
    public interface IStravaAuthClient
    {
        public IRestClient RestClient { get; }

        public Task<TokenResponse> GetAuthToken(string code);

    }
}
