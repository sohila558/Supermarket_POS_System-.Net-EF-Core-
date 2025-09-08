using Microsoft.AspNetCore.Mvc;
using MySuperMarket.DTOs;

namespace MySuperMarket.Repository.RepoRoles
{
    public interface IRepoRole
    {
        Task GetAllRolesAsync();
        Task CreateRoleAsync(string name);
        Task GetAllUsers();
        Task<bool> AddUserToRole(string email, string roleName);
        Task<bool> GetUserRoles(string email);
        Task<bool> RemoveUserFromRole(string email, string roleName);

    }
}
