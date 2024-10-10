using User_API.DataAccessObject;
using User_API.PasswordHandler;
using User_API.Services;

namespace User_API.Configurations
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Dependency Injection configurations for services.

            services.AddScoped<IUserService, UserServiceClass>();
            services.AddScoped<IClientService, ClientServiceClass>();
            services.AddScoped<IUserDao, UserDaoClass>();
            services.AddScoped<IClientDao, ClientDaoClass>();
            services.AddScoped<IClientVerifyDao, ClientVerifyClass>();
            services.AddScoped<TokenGeneratorService>();
            services.AddScoped<PasswordHasher256, PasswordHasher>();

            return services;
        }
    }
}