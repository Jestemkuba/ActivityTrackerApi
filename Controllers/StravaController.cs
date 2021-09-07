using ActivityTrackerApi.Clients;
using ActivityTrackerApi.Data.DTOs.Activities;
using ActivityTrackerApi.Data.Models;
using ActivityTrackerApi.Data.Repositories.Contracts;
using ActivityTrackerApi.MappingExtensions.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public StravaController(IStravaClient stravaClient, IRepositoryWrapper repositoryWrapper, UserManager<ApplicationUser> userManager)
        {
            _stravaClient = stravaClient;
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("SyncStravaActivities")]
        public async Task<IActionResult> SynchroniseActivitiesWithStrava([FromHeader] string stravaAuthToken)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var stravaActivities = await _stravaClient.GetActivities(stravaAuthToken);
                var activities = await _repositoryWrapper.Activity.FindByCondition(a => a.User.UserName == user.UserName);
                foreach (var stravaActivity in stravaActivities)
                {
                    if (!activities.Any(a => a.StravaId == stravaActivity.Id))
                    {
                        var activity = stravaActivity.ToActivity();
                        activity.User = user;
                        _repositoryWrapper.Activity.Create(activity);
                    }
                }
                await _repositoryWrapper.Save();
                return Ok("Updated");
            }
            catch (Exception ex)
            {
                return BadRequest("Strava auth token invalid");
            }
        }
    }
}
