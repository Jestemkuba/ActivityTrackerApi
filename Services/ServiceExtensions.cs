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
    }
}
