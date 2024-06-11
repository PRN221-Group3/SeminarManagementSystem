using DataAccess.DAO;
using Repositories;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            AddDAO(services);
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection AddDAO(this IServiceCollection services)
        {
            services.AddScoped<UserDAO>();
            return services;
        }
    }
}