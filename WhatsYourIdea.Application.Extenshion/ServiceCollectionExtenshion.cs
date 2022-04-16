using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatsYourIdea.Applications.Auth;
using WhatsYourIdea.Applications.DTO;

namespace WhatsYourIdea.Application.Extension
{
    public static class ServiceCollectionExtenshion
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        public static IServiceCollection AddApplicationAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LoginPath = "/account/logout";
                options.SlidingExpiration = true;

                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
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