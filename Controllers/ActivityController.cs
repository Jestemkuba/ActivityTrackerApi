using ActivityTrackerApi.Data;
using ActivityTrackerApi.Data.DTOs.Activities;
using ActivityTrackerApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetUserActivities(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
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
    }
}
