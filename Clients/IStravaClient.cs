using ActivityTrackerApi.Data.Models;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Clients
{
    public interface IStravaClient
    {
        public IRestClient RestClient { get; }
        public Task<IEnumerable<Activity>> GetActivities(string token);
    }
}
