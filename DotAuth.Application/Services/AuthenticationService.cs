using DotAuth.Application.Contracts.Requests;
using DotAuth.Application.Contracts.Responses;
using DotAuth.Application.Interfaces;
using DotAuth.Domain.Entities;

namespace DotAuth.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public AuthenticationService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
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
            return new RegisterResponse
            {
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
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
                    throw new Exception("Invalid credentials.");
                }

                if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
                {
                    throw new Exception("Invalid credentials.");
                }
            }

            // Generate tokens
            var accessToken = _jwtProvider.GenerateAccessToken(user);
            var refreshToken = _jwtProvider.GenerateRefreshToken();
            // Return response
            return new LoginResponse
            {
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
