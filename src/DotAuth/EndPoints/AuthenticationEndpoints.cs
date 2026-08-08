using DotAuth.Application.Contracts.Requests;
using DotAuth.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DotAuth;
public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapDotAuth(
    this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/signup",
            async (
                RegisterRequest request,
                IAuthenticationService authenticationService) =>
            {
                var result = await authenticationService.RegisterAsync(request);
                if(!result.IsSuccess)
                {
                    return Results.BadRequest(result.ErrorMessage);
                }

                return Results.Ok(result.Value);
            })
            .WithName("Register")
            .WithTags("Authentication");
        
        app.MapPost("/auth/login", async ( LoginRequest request, IAuthenticationService authenticationService) =>
            {
                var result = await authenticationService.LoginAsync(request);
                if(!result.IsSuccess)
                {
                    return Results.BadRequest(result.ErrorMessage);
                }

                return Results.Ok(result.Value);
            })
            .WithName("Login")
            .WithTags("Authentication");

        app.MapGet("auth/me",
            [Authorize] async (
                ClaimsPrincipal user,
                IAuthenticationService authenticationService) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userIdClaim))
                    return Results.Unauthorized();

                var result = await authenticationService.GetCurrentUserAsync(Guid.Parse(userIdClaim));
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(result.ErrorMessage);
                }

                return Results.Ok(result.Value);
            })
            .WithName("CurrentUser")
            .WithTags("Authentication");

        app.MapPost("/auth/refresh", async (RefreshTokenRequest request, IAuthenticationService authenticationService) =>
            {
                var result = await authenticationService.RefreshTokenAsync(request);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(result.ErrorMessage);
                }
                return Results.Ok(result.Value);
            })
            .WithName("RefreshToken")
            .WithTags("Authentication");

        app.MapPost("/auth/logout", async (LogoutRequest logoutRequest, IAuthenticationService authenticationService) =>
            {
                await authenticationService.LogoutAsync(logoutRequest);
                return Results.NoContent();
            })
            .WithName("Logout")
            .WithTags("Authentication");

        return app;
    }

}
