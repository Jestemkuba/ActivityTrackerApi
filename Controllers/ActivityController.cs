using ActivityTrackerApi.Clients;
using ActivityTrackerApi.Data;
using ActivityTrackerApi.Data.DTOs.Activities;
using ActivityTrackerApi.Data.Models;
using ActivityTrackerApi.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActivityController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ActivityController(UserManager<ApplicationUser> userManager, IRepositoryWrapper repositoryWrapper)
        {
            _userManager = userManager;
            _repositoryWrapper = repositoryWrapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserActivities()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
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
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            activity.User = user;
            _repositoryWrapper.Activity.Create(activity);
            await _repositoryWrapper.Save();
            return Ok(activity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAcitivity(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var activity = _repositoryWrapper.Activity.FindByCondition(a => a.Id == id && a.User.Id == user.Id)
                                                        .Result
                                                        .FirstOrDefault();

            if (activity is null)
            {
                return NotFound("Activity not found");
            }

            _repositoryWrapper.Activity.Delete(activity);
            await _repositoryWrapper.Save();

            return Ok("Resource deleted");
        }      
    }
}
