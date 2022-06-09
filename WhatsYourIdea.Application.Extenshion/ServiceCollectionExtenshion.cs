using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatsYourIdea.Applications.Auth;
using WhatsYourIdea.Applications.DTO;
using WhatsYourIdea.Applications.Services;
using WhatsYourIdea.Applications.Services.Configurations;
using WhatsYourIdea.Applications.Services.Interfaces;
using WhatsYourIdea.Applications.Services.Services;

namespace WhatsYourIdea.Application.Extension
{
    public static class ServiceCollectionExtenshion
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.Configure<FileStorageSettings>(config =>
            {
                config.IdeaImagesFolderPath = Path.Combine(
                    environment.WebRootPath,
                    configuration.GetSection("FileStorageSettings:ideafilesFileFolder").Value
                    );
                config.IdeaImagesFolderName = configuration.GetSection("FileStorageSettings:ideafilesFileFolder").Value;
            });

            services.AddSingleton<IFileStorageService, FileStorageService>();
            services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();

            return services;
        }

        public static IServiceCollection AddApplicationAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LoginPath = "/account/logout";
                options.SlidingExpiration = true;

                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(2);

                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            services.AddScoped<AuthService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection AddApplicationAutoMapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(config =>
            {
                config.AddMaps(typeof(DTOAssembly).Assembly);
            });

            return services;
        }
    }
}