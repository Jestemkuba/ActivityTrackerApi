
using ActivityTrackerApi.Data;
using ActivityTrackerApi.Data.DTOs;
using ActivityTrackerApi.Data.Models;
using ActivityTrackerApi.Data.Repositories.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext dbContext, IRepositoryWrapper repositoryWrapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        [HttpGet]
        [Route("/api/users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(RegisterUserDto user)
        {
            var userToRegister = _mapper.Map<ApplicationUser>(user);
            var result = await _userManager.CreateAsync(userToRegister, user.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(userToRegister);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser([FromBody] JsonPatchDocument<ApplicationUser> patchEntity)
        {
            var user = await _userManager.FindByIdAsync(User.Identity.Name);
            if (user is null)
            {
                return NotFound();
            }

            patchEntity.ApplyTo(user);
            await _userManager.UpdateAsync(user);
            return Ok(user);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user is null)
            {
                return NotFound();
            }

            var activites = await _repositoryWrapper.Activity.FindByCondition(a => a.User.Id == user.Id);
            foreach (var activity in activites)
            {
                _repositoryWrapper.Activity.Delete(activity);
            };
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok(result);
        }       
    }
}
