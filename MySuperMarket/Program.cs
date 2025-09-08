
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySuperMarket.Helpers;
using MySuperMarket.Data;
using MySuperMarket.Models;
using MySuperMarket.Repository.GenericRepository;
using MySuperMarket.Repository.InvoiceRepository;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MySuperMarket.Repository.IdentityRepository;
using MySuperMarket.Controllers;
using MySuperMarket.Repository.RepoRoles;
using System.Data;

namespace MySuperMarket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddTransient<IRepoInvoices, RepoInvoices>();
            builder.Services.AddTransient<IRepoIdentity, RepoIdentity>();
            builder.Services.AddTransient<IRepoRole, RepoRole>();

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("myconnection")));

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.User.RequireUniqueEmail = false;
                //options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            //builder.Services.AddIdentityApiEndpoints<AppUser>()
            //   .AddEntityFrameworkStores<ApplicationDbContext>()
            //   .AddSignInManager<SignInManager<AppUser>>()
            //   .AddUserManager<UserManager<AppUser>>()
            //   .AddDefaultTokenProviders();

            var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

            if (jwtOptions == null)
            {
                throw new InvalidOperationException("Jwt options is not configured.");
            }

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
              {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
                    };
              });


            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
           // app.MapIdentityApi<AppUser>();

            app.MapControllers();

            app.Run();
        }
    }
}
