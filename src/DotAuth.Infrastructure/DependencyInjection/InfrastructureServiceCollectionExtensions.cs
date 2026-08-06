using DotAuth.Application.Interfaces;
using DotAuth.Infrastructure.Persistence.Context;
using DotAuth.Infrastructure.Persistence.Repositories;
using DotAuth.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddDotAuthInfrastructure(
        this IServiceCollection services,
        string connectionString)
        {
            services.AddDbContext<DotAuthDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }
    }
}
