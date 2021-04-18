using ActivityTrackerApi.Clients;
using ActivityTrackerApi.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Controllers
{
    [ApiController]
    [Route("api/strava")]
    public class StravaController : ControllerBase
    {
        private readonly IStravaClient _stravaClient;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public StravaController(IStravaClient stravaClient, IRepositoryWrapper repositoryWrapper)
        {
            _stravaClient = stravaClient;
            _repositoryWrapper = repositoryWrapper;
        }

        [HttpPost]
        [Route("SyncStravaActivities")]
        public async Task<IActionResult> SynchroniseActivitiesWithStrava([FromHeader] string stravaAuthToken)
        {
            var stravaActivities = await _stravaClient.GetActivities(stravaAuthToken);
            var activities = await _repositoryWrapper.Activity.FindAll();
            foreach (var stravaActivity in stravaActivities)
            {
                if (!activities.Any(a => a.Id == stravaActivity.Id))
                {
                    _repositoryWrapper.Activity.Create(stravaActivity);
                }
            }
            await _repositoryWrapper.Save();
            return Ok("Updated");
        }
    }
}
