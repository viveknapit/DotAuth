using DotAuth.Application.Contracts.Requests;
using DotAuth.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

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

            return app;
        }
    }
}
