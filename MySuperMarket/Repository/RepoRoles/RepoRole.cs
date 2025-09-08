using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySuperMarket.Controllers;
using MySuperMarket.Data;
using MySuperMarket.DTOs;
using MySuperMarket.Models;

namespace MySuperMarket.Repository.RepoRoles
{
    public class RepoRole : IRepoRole
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RolesController> _logger;
        public RepoRole(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RolesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task<bool> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.LogInformation($"The user with the {email} does not exist");
                return false;
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                _logger.LogInformation($"The role {roleName} does not exist");

                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, email);

            return true;
        }

        public async Task CreateRoleAsync(string name)
        {
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));
            }
        }

        public async Task GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
        }

        public async Task GetAllUsers()
        {
            var Users = await _userManager.Users.ToListAsync();
        }

        public async Task<bool> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.LogInformation($"The user with the {email} does not exist");
                return false;
            }

            var roles = await _userManager.GetRolesAsync(user);

            return true;
        }

        public async Task<bool> RemoveUserFromRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.LogInformation($"The user with the {email} does not exist");
                return false;
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                _logger.LogInformation($"The role {email} does not exist");

                return false;
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            return true;
        }
    }
}
