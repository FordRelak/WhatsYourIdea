using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WhatsYourIdea.Applications.Hasher
{
    public static class ServiceCollectionExtenshion
    {
        public static IServiceCollection AddHasher(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection("Hasher").Get<HasherSetting>());
            services.AddSingleton(provider => new HasherService(provider.GetRequiredService<HasherSetting>()));
            return services;
        }
    }
}