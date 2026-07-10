using DotAuth.Application.Interfaces;
using DotAuth.Application.Services;
using DotAuth.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DotAuth.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDotAuth(
        this IServiceCollection services,
        Action<DotAuthOptions> configure)
        {
            var options = new DotAuthOptions();

            configure(options);

            services.AddDotAuthInfrastructure(options.ConnectionString);

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
