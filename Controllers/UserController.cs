
using ActivityTrackerApi.Data;
using ActivityTrackerApi.Data.DTOs;
using ActivityTrackerApi.Data.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser([FromRoute] int id, [FromBody] JsonPatchDocument<ApplicationUser> patchEntity)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                return NotFound();
            }

            patchEntity.ApplyTo(user);
            await _userManager.UpdateAsync(user);
            return Ok(user);
        }
    }
}
