using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySuperMarket.Data;
using MySuperMarket.Models;
using MySuperMarket.Repository.RepoRoles;

namespace MySuperMarket.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRepoRole _repoRole;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;


        public RolesController(IRepoRole repoRole, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _repoRole = repoRole;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("Roles")]
        public IActionResult GetAllRoles() 
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync(string name) 
        {
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (!roleExist) 
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));
            }

            return BadRequest(new { error = "Role already exist" });
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers() 
        {
            var Users = await _userManager.Users.ToListAsync();
            return Ok(Users);
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName) 
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) 
            {
                return BadRequest();
            }
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist) 
            {
                return BadRequest();
            }
            var result = await _userManager.AddToRoleAsync(user, email);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<bool> GetUserRoles(string email) 
        {
            return await _repoRole.GetUserRoles(email);
        }

        [HttpPut]
        [Route("RemoveUserFromRole")]
        public async Task<bool> RemoveUserFromRole(string email, string roleName) 
        {
            return await _repoRole.RemoveUserFromRole(email, roleName);
        }
    }
}
