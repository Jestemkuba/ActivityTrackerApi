using ActivityTrackerApi.Clients;
using ActivityTrackerApi.Data.Models;
using ActivityTrackerApi.Data.Repositories.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Controllers
{
    [ApiController]
    [Route("api/strava")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StravaController : ControllerBase
    {
        private readonly IStravaClient _stravaClient;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public StravaController(IStravaClient stravaClient, IRepositoryWrapper repositoryWrapper, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _stravaClient = stravaClient;
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("SyncStravaActivities")]
        public async Task<IActionResult> SynchroniseActivitiesWithStrava([FromHeader] string stravaAuthToken)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var stravaActivities = await _stravaClient.GetActivities(stravaAuthToken);
            var activities = await _repositoryWrapper.Activity.FindByCondition(a => a.User.UserName == user.UserName);
            foreach (var stravaActivity in stravaActivities)
            {
                if (!activities.Any(a => a.StravaId == stravaActivity.Id))
                {
                    var activity = _mapper.Map<Activity>(stravaActivity);
                    activity.User = user;
                    _repositoryWrapper.Activity.Create(activity);
                }
            }
            await _repositoryWrapper.Save();
            return Ok("Updated");
        }
    }
}
