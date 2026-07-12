using DotAuth.Application.Contracts.Requests;
using DotAuth.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DotAuth.Presentation.EndPoints
{
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
                    var response = await authenticationService.RegisterAsync(request);

                    return Results.Ok(response);
                })
                .WithName("Register")
                .WithTags("Authentication");
            
            app.MapPost("/auth/login", async ( LoginRequest request, IAuthenticationService authenticationService) =>
                {
                    var response = await authenticationService.LoginAsync(request);
                    return Results.Ok(response);
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

                    var response = await authenticationService.GetCurrentUserAsync(Guid.Parse(userIdClaim));

                    return Results.Ok(response);
                })
                .WithName("CurrentUser")
                .WithTags("Authentication");

            return app;
        }

    }
}
