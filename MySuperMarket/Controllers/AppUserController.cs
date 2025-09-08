using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySuperMarket.Data;
using MySuperMarket.DTOs;
using MySuperMarket.Models;
using MySuperMarket.Repository.IdentityRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MySuperMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IRepoIdentity _identityRepo;

        public AppUserController(IRepoIdentity identityRepo)
        {
            _identityRepo = identityRepo;
        }

        [HttpPost]
        [Route("auth")]
        public async Task<IActionResult> AuthenticateUser(AuthenticationRequest request)
        {
            var accessToken = await _identityRepo.SignIn(request.UserName, request.Password);

            return Ok(accessToken);
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUser(CreateUserDTO request)
        {
            var isUserCreated = await _identityRepo.AddAsync(request);

            if (isUserCreated)
                return Ok();
            else
                return BadRequest("Cannot create user");
        }

        [HttpDelete]
        public async Task<bool> DeleteUserAsync(string id) 
        {
            return await _identityRepo.DeleteUserAsync(id);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IEnumerable<GetUserDTO>> GetAllAsync() 
        {
            return await _identityRepo.GetAllAsync();
        }

        [HttpGet]
        [Route("Id")]
        public async Task<AppUser?> GetUserByIdAsync(string id) 
        {
            return await _identityRepo.GetUserByIdAsync(id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(string id, UpdateUserDTO dto) 
        {
            await _identityRepo.UpdateUserAsync(id, dto);
            return Ok();
        }
    }
}
