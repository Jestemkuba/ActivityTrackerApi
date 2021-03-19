using ActivityTrackerApi.Data;
using ActivityTrackerApi.Data.DTOs.Activities;
using ActivityTrackerApi.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Controllers
{
    [ApiController]
    [Route("/api/activities")]
    public class ActivityController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ActivityController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddActivity(Activity activity)
        {
            await _dbContext.Activities.AddAsync(activity);
            await _dbContext.SaveChangesAsync();
            return Ok(activity);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserActivities(string id)
        {
            if (!int.TryParse(id, out var intId))
                return BadRequest("User ID must be integer number");

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound("User not found");
            }
            var activities = await _dbContext.Activities.Where(a => a.User.Equals(user)).ToListAsync();

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
        public async Task<IActionResult> DeleteAcitivity([FromBody]object jsonString)
        {
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString.ToString());
            var id = jsonObject["id"];

            if (!int.TryParse(id, out var intId))
                return BadRequest("User ID must be integer number");

            try
            {
                var activity = _dbContext.Activities.Where(a => a.Id == intId).FirstOrDefault();
                if (activity == null)
                {
                    return NotFound("Activity not found");
                }
                var result = _dbContext.Activities.Remove(activity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok("Resource deleted");
        }
    }
}
