using DotAuth.Application.Common.Results;
using DotAuth.Application.Contracts.Requests;
using DotAuth.Application.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request);

        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);

        Task<Result<CurrentUserResponse>> GetCurrentUserAsync(Guid UserId);
    }
}
