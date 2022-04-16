using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatsYourIdea.Infrastructure.Identity;

namespace WhatsYourIdea.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EfDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PosgresConnectionString")));
            services.AddScoped<IUnitOfWorkInfrastructure, UnitOfWorkInfrastructure>();
            return services;
        }

        public static IServiceCollection AddInfrastructureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<EfDbContext>();

            return services;
        }
    }
}