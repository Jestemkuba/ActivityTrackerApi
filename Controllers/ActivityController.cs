using ActivityTrackerApi.Clients;
using ActivityTrackerApi.Data;
using ActivityTrackerApi.Data.DTOs.Activities;
using ActivityTrackerApi.Data.Models;
using ActivityTrackerApi.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Controllers
{
    [ApiController]
    [Route("/api/activities")]
    public class ActivityController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStravaClient _stravaClient;

        public ActivityController(UserManager<ApplicationUser> userManager, IRepositoryWrapper repositoryWrapper, IStravaClient stravaClient)
        {
            _userManager = userManager;
            _repositoryWrapper = repositoryWrapper;
            _stravaClient = stravaClient;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserActivities(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            var activities = await _repositoryWrapper.Activity.FindByCondition(a => a.User.Equals(user));
            
            return Ok(activities);
        }

        [HttpPost]
        public async Task<IActionResult> AddActivity(Activity activity)
        {
            _repositoryWrapper.Activity.Create(activity);
            await _repositoryWrapper.Save();
            return Ok(activity);
        }  

        [HttpDelete]
        public async Task<IActionResult> DeleteAcitivity([FromBody] object jsonString)
        {
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString.ToString());
            var id = jsonObject["id"];

            if (!int.TryParse(id, out var intId))
            {
                return BadRequest("User ID must be integer number");
            }

            var activity = _repositoryWrapper.Activity.FindByCondition(a => a.Id == intId).Result.FirstOrDefault();
            if (activity == null)
            {
                return NotFound("Activity not found");
            }
            await _repositoryWrapper.Save();

            return Ok("Resource deleted");
        }

        [HttpPost]
        [ActionName("StravaSync")]
        public async Task<IActionResult> SynchroniseActivitiesWithStrava([FromHeader]string stravaAuthToken)
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
