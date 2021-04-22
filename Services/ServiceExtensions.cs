using ActivityTrackerApi.Clients;
using ActivityTrackerApi.Data.Repositories;
using ActivityTrackerApi.Data.Repositories.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityTrackerApi.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
        public static void ConfigureStravaClients(this IServiceCollection services)
        {
            services.AddScoped<IStravaAuthClient, StravaAuthClient>();
            services.AddScoped<IStravaClient, StravaClient>();
        }
    }
}
