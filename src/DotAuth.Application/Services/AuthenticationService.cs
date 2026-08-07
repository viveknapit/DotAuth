using DotAuth.Application.Common.Results;
using DotAuth.Application.Contracts.Requests;
using DotAuth.Application.Contracts.Responses;
using DotAuth.Application.Interfaces;
using DotAuth.Domain.Entities;
using DotAuth.Application.Common.Security;

namespace DotAuth.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        #region Constructor
        public AuthenticationService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _refreshTokenRepository = refreshTokenRepository;
        }
        #endregion

        #region Register method
        public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request)
        {
            // Check if email already exists
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var emailExists = await _userRepository.ExistsByEmailAsync(request.Email);

                if (emailExists)
                    throw new Exception("Email already exists.");
            }

            // Check if phone number already exists
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var phoneExists = await _userRepository.ExistsByPhoneAsync(request.PhoneNumber);

                if (phoneExists)
                    throw new Exception("Phone number already exists.");
            }

            // Check if username already exists
            if (!string.IsNullOrWhiteSpace(request.UserName))
            {
                var usernameExists = await _userRepository.ExistsByUserNameAsync(request.UserName);

                if (usernameExists)
                    throw new Exception("Username already exists.");
            }

            // Hash password
            var passwordHash = _passwordHasher.Hash(request.Password);

            // Create user
            var user = DotAuthUser.Create(
                request.UserName,
                request.Email,
                request.PhoneNumber,
                passwordHash,
                request.FirstName,
                request.LastName);

            // Save
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Generate tokens
            var accessToken = _jwtProvider.GenerateAccessToken(user);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            // Return response
            return Result<RegisterResponse>.Success(new RegisterResponse
            {
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        #endregion

        #region Login method
        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
        {
            DotAuthUser? user = null;
            if (!string.IsNullOrEmpty(request.Login))
            {
                if (request.Login.Contains('@'))
                {
                    user = await _userRepository.FindByEmailAsync(request.Login);

                }
                else if (request.Login.All(char.IsDigit))
                {
                    user = await _userRepository.FindByPhoneAsync(request.Login);
                }
                else
                {
                    user = await _userRepository.FindByUsernameAsync(request.Login);
                }

                if (user == null)
                {
                    return Result<LoginResponse>.Failure("User does not exist.");
                }

                if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
                {
                    return Result<LoginResponse>.Failure("Invalid credentials.");
                }
            }

            // Generate tokens
            var accessToken = _jwtProvider.GenerateAccessToken(user);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            //store refresh token in the database
            var hashedRefreshToken = TokenHashing.Hash(refreshToken);

            var refreshTokenEntity = RefreshToken.Create(user.Id, hashedRefreshToken, 7);

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            // Return response
            return Result<LoginResponse>.Success(new LoginResponse
            {
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        #endregion

        #region Current User method
        public async Task<Result<CurrentUserResponse>> GetCurrentUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            return Result<CurrentUserResponse>.Success(new CurrentUserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }
        #endregion

        public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var hashedRefreshToken = TokenHashing.Hash(request.Token);
            var refreshTokenEntity = await _refreshTokenRepository.GetByHashAsync(hashedRefreshToken);
            if (refreshTokenEntity == null || refreshTokenEntity.IsExpired || refreshTokenEntity.IsRevoked)
            {
                return Result<RefreshTokenResponse>.Failure("Invalid or expired refresh token.");
            }
            var user = await _userRepository.GetByIdAsync(refreshTokenEntity.UserId);
            if (user == null)
            {
                return Result<RefreshTokenResponse>.Failure("User not found.");
            }
            // Generate new tokens
            var newAccessToken = _jwtProvider.GenerateAccessToken(user);
            var newRefreshToken = _jwtProvider.GenerateRefreshToken();
            // Revoke the old refresh token
            refreshTokenEntity.Revoke();
            await _refreshTokenRepository.SaveChangesAsync();
            // Store the new refresh token in the database
            var newHashedRefreshToken = TokenHashing.Hash(newRefreshToken);
            var newRefreshTokenEntity = RefreshToken.Create(user.Id, newHashedRefreshToken, 7);
            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();
            return Result<RefreshTokenResponse>.Success(new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        public async Task LogoutAsync(LogoutRequest request)
        {
            var hashedRefreshToken = TokenHashing.Hash(request.RefreshToken);
            var refreshTokenEntity = await _refreshTokenRepository.GetByHashAsync(hashedRefreshToken);
            if (refreshTokenEntity != null)
            {
                refreshTokenEntity.Revoke();
                await _refreshTokenRepository.SaveChangesAsync();
            }
        }
    }
}
