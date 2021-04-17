using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Clients
{
    public interface IStravaClient
    {
        public IRestClient RestClient { get; }

        public Task<object> GetToken();
    }
}
