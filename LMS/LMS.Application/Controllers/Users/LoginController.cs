using LMS.Application.common;
using LMS.Application.DTOs.RequestDTOs;
using LMS.Application.DTOs.ResponseDTOs;
using LMS.Application.Services;
using LMS.Application.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Application.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly TokenService _tokenService;

        public LoginController(
            LoginService loginService,
            TokenService tokenService)
        {
            _loginService = loginService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Logs in a user using their email and password.
        /// </summary>
        /// <param name="loginDTO">The login request containing email and password.</param>
        /// <returns>
        /// Returns an HTTP 200 response with a JWT token if login is successful.
        /// Returns an HTTP 400 response if the email is missing or the user is not found.
        /// Returns an HTTP 401 response if authentication fails.
        /// </returns>
        [HttpPost("login")]
        public async Task<ActionResult> LogIn(LoginDTO loginDTO)
        {
            // Validate email input
            if (string.IsNullOrWhiteSpace(loginDTO.Email))
            {
                return BadRequest(Result.Failure("Email cannot be empty."));
            }

            // Attempt to log in using password
            var result = await _loginService.LoginWithPassword(loginDTO);

            if (!result.IsSuccess)
            {
                return Unauthorized(result); // Return Unauthorized if authentication fails
            }

            // Retrieve user details after successful login
            var user = await _loginService.GetUserByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                return BadRequest(Result.Failure("User not found."));
            }

            // Generate Access Token and Refresh Token
            var token = _tokenService.GenerateAccessToken(user);
            await _tokenService.GenerateRefreshTokenAsync(user);

            // Return response with token
            return Ok(new AuthorizeToken
            {
                Status = true,
                Message = result.Message,
                Token = token
            });
        }

        /// <summary>
        /// Generates a new access token for a user if the refresh token is still valid.
        /// </summary>
        /// <param name="userId">The ID of the user requesting a new token.</param>
        /// <returns>
        /// Returns an HTTP 200 response with a new JWT token if the refresh token is valid.
        /// Returns an HTTP 404 response if the user is not found.
        /// Returns an HTTP 401 response if the refresh token is expired.
        /// </returns>
        [HttpPost("get-token/{userId}")]
        public async Task<ActionResult> GetToken(int userId)
        {
            // Retrieve the user by userId
            var user = await _loginService.GetUserAsync(userId);

            if (user == null)
            {
                return NotFound(Result.Failure("User not found."));
            }

            // Validate the existing refresh token for the user
            var result = await _tokenService.CheckOldToken(user);

            if (result.IsSuccess)
            {
                // Generate a new access token
                var token = _tokenService.GenerateAccessToken(user);

                return Ok(new AuthorizeToken
                {
                    Status = true,
                    Message = result.Message,
                    Token = token
                });
            }

            return Unauthorized(Result.Failure("Refresh token is expired."));
        }

    }
}
