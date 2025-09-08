using MySuperMarket.DTOs;
using MySuperMarket.Models;

namespace MySuperMarket.Repository.IdentityRepository
{
    public interface IRepoIdentity
    {
        Task<bool> AddAsync(CreateUserDTO dto);
        Task<IEnumerable<GetUserDTO>> GetAllAsync();
        Task<AppUser?> GetUserByIdAsync(string id);
        Task UpdateUserAsync(string id, UpdateUserDTO dto);
        Task<bool> DeleteUserAsync(string id);
        Task<string> SignIn(string username, string password);
        
    }
}
