using DotAuth.Application.Contracts.Requests;
using DotAuth.Application.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);

        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
