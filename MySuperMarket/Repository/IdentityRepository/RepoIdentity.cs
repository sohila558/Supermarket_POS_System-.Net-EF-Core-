using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySuperMarket.Data;
using MySuperMarket.DTOs;
using MySuperMarket.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MySuperMarket.Repository.IdentityRepository
{
    public class RepoIdentity : IRepoIdentity
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RepoIdentity> _logger;
        protected readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public RepoIdentity(ApplicationDbContext context, 
            IMapper mapper,
            IConfiguration configuration,
            UserManager<AppUser> userManager
            ,RoleManager<IdentityRole> roleManager
            ,ILogger<RepoIdentity> logger) 
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;   
        }
        public async Task<bool> AddAsync(CreateUserDTO dto)
        {
            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> SignIn(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return string.Empty;
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

            if (isPasswordValid == false)
            {
                return string.Empty;
            }

            var jwtOptions = _configuration.GetSection("Jwt").Get<JwtOptions>();

            if (jwtOptions == null)
            {
                throw new InvalidOperationException("Jwt options is not configured.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.NameIdentifier, user.UserName),
                    new (ClaimTypes.Email, user.Email)
                })
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return accessToken;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return false;
            }

            var deleteResult = await _userManager.DeleteAsync(user);

            if (deleteResult.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<GetUserDTO>> GetAllAsync()
        {
            var allUsers = await _userManager.Users.ToListAsync();

            var usersDto = _mapper.Map<List<GetUserDTO>>(allUsers);

            return usersDto;
        }

        public async Task<AppUser?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task UpdateUserAsync(string id, UpdateUserDTO dto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return;
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.UserName;

            await _userManager.UpdateAsync(user);
        }

    }
}
