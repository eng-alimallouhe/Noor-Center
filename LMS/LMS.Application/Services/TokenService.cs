using LMS.Application.common;
using LMS.Domain.Entities.Users;
using LMS.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LMS.Application.Services.Users
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public TokenService(
            IConfiguration configuration,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
        }

        /// <summary>
        /// Generates an access token (JWT) for a given user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <returns>A JWT access token as a string.</returns>
        /// <remarks>
        /// This method creates a JWT token containing user-related claims such as UserId, Email, and Role.
        /// The token is signed using HMAC SHA256 and expires after 30 minutes.
        /// </remarks>
        public string GenerateAccessToken(User user)
        {
            // Create a security key from the configured secret key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));

            // Generate signing credentials using HMAC SHA256 algorithm
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define user claims to be included in the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()), // User ID
                new Claim(JwtRegisteredClaimNames.Email, user.Email), // User email
                new Claim(ClaimTypes.Role, user.Role.RoleType), // User role
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique identifier for the token
            };

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), // Token expiration time
                signingCredentials: credentials
            );

            // Serialize and return the token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generates a refresh token for a given user.
        /// </summary>
        /// <param name="user">The user for whom the refresh token is generated.</param>
        /// <returns>A result indicating success or failure of refresh token generation.</returns>
        /// <remarks>
        /// This method creates a cryptographically secure random refresh token, stores it in the database, 
        /// and sets its expiration date to 7 days.
        /// </remarks>
        public async Task<Result> GenerateRefreshTokenAsync(User user)
        {
            // Ensure the user exists before generating a token
            if (user == null)
            {
                return Result.Failure("User not found");
            }

            // Create a 64-byte secure random token
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            // Create a new refresh token object
            var token = new RefreshToken
            {
                UserId = user.UserId,
                Token = Convert.ToBase64String(randomBytes), // Convert to Base64 string
                Expiration = DateTime.UtcNow.AddDays(7), // Token valid for 7 days
                CreatedAt = DateTime.UtcNow
            };

            // Remove any previous refresh token for this user
            await _refreshTokenRepository.DeleteAsync(user.UserId);

            // Store the new refresh token in the database
            await _refreshTokenRepository.AddAsync(token);

            return Result.Success("Refresh token generated successfully");
        }

        /// <summary>
        /// Checks if the user has a valid refresh token.
        /// </summary>
        /// <param name="user">The user whose refresh token needs verification.</param>
        /// <returns>A result indicating whether the refresh token is valid or expired.</returns>
        /// <remarks>
        /// If the refresh token is expired, a new refresh token is generated.
        /// </remarks>
        public async Task<Result> CheckOldToken(User user)
        {
            // Ensure the user exists before checking the token
            if (user == null)
            {
                return Result.Failure("User not found");
            }

            // Retrieve the user's current refresh token
            var oldToken = user.RefreshToken;

            // Check if a refresh token exists
            if (oldToken == null)
            {
                return Result.Failure("Refresh token not found");
            }

            // Check if the token has expired
            if (oldToken.Expiration <= DateTime.UtcNow)
            {
                return Result.Failure("Refresh token is expired");
            }

            // Generate a new refresh token if the old one is still valid
            await GenerateRefreshTokenAsync(user);

            return Result.Success("Refresh token is valid");
        }

    }
}
