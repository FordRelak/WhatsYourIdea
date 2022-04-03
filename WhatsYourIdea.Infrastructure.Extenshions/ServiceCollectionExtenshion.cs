using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WhatsYourIdea.Infrastructure.Extenshions
{
    public static class ServiceCollectionExtenshion
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EfDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("whatsyouridea")));
            services.AddScoped<IUnitOfWorkInfrastructure, UnitOfWorkInfrastructure>();
            return services;
        }
    }
}