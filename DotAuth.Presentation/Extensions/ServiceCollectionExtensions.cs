using DotAuth.Application.Interfaces;
using DotAuth.Application.Services;
using DotAuth.Infrastructure.DependencyInjection;
using DotAuth.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DotAuth.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDotAuth(
        this IServiceCollection services,
        Action<DotAuthOptions> configure)
        {
            var dotAuthOptions = new DotAuthOptions();

            configure(dotAuthOptions);

            services.AddDotAuthInfrastructure(dotAuthOptions.ConnectionString);

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.Configure<JwtOptions>(jwt =>
            {
                jwt.SecretKey = dotAuthOptions.Jwt.SecretKey;
                jwt.Issuer = dotAuthOptions.Jwt.Issuer;
                jwt.Audience = dotAuthOptions.Jwt.Audience;
                jwt.AccessTokenExpirationMinutes =
                    dotAuthOptions.Jwt.AccessTokenExpirationMinutes;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtOptions =>
                {
                    JwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = dotAuthOptions.Jwt.Issuer,

                        ValidateAudience = true,
                        ValidAudience = dotAuthOptions.Jwt.Audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(dotAuthOptions.Jwt.SecretKey)),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}
