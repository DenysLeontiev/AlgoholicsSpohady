using System.Text;
using API.ActionFilters;
using API.Data;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;

namespace API.ExtensionMethods
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(opts =>
            {
                opts.UseSqlite(connectionString);
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                // options.Password.RequireDigit = false;
                // options.Password.RequiredLength = 6; // TODO: Change on production (14.05.2023)
                // options.Password.RequireLowercase = false;
                // options.Password.RequireNonAlphanumeric = false;
                // options.Password.RequireUppercase = false;
                // options.Password.Re
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<DataContext>();
        }

        public static void ConfigureAuthentification(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenHandler, Services.TokenHandler>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IQrCodeGenerator, QrCodeGenerator>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddCors();
        }

        public static void ConfigureCloudinaryAccount(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CloudinaryAccount>(configuration.GetSection("CloudinarySettings"));
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void ConfigureDataShapper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<User>, DataShaper<User>>();
        }

        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<MemoryWithMemoryIdExists>(); // TODO: Check is that's correct
            services.AddScoped<UserWithUserIdExists>();
            services.AddScoped<RemoveUserFromMemoryAttribute>();
        }
    }
}