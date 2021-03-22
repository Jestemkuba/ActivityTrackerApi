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

        public ActivityController(UserManager<ApplicationUser> userManager, IRepositoryWrapper repositoryWrapper)
        {
            _userManager = userManager;
            _repositoryWrapper = repositoryWrapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddActivity(Activity activity)
        {
            await _repositoryWrapper.Activity.Create(activity);
            await _repositoryWrapper.Save();
            return Ok(activity);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserActivities(string id)
        {
            if (!int.TryParse(id, out var intId))
            {
                return BadRequest("User ID must be integer number");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            var activities = await _repositoryWrapper.Activity.FindByCondition(a => a.User.Equals(user));
            List<ActivityDto> result = new List<ActivityDto>();
            foreach (var activity in activities)
            {
                result.Add(new ActivityDto()
                {
                    Id = activity.Id,
                    Description = activity.Description
                });
            }
            return Ok(result);
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
    }
}
