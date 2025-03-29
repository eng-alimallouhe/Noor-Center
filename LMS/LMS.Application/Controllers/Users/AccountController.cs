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
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly TokenService _tokenService;

        public AccountController(
            AccountService accountService,
            TokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Verifies if an email is already registered in the system.
        /// </summary>
        /// <param name="email">The email address to verify.</param>
        /// <returns>
        /// Returns an HTTP 200 response if the email is available, otherwise an HTTP 400 response.
        /// </returns>
        /// <remarks>
        /// This method checks if the given email exists in the database and returns a success or failure result.
        /// </remarks>
        [HttpPost("verify-email")]
        public async Task<ActionResult> VerifyEmail(string email)
        {
            // Call the account service to check if the email is registered
            var result = await _accountService.VerifyEmail(email);

            // If the email is available, return a success response
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            // Otherwise, return a failure response
            return BadRequest(result);
        }

        /// <summary>
        /// Verifies if a username is already registered in the system.
        /// </summary>
        /// <param name="username">The username to verify.</param>
        /// <returns>
        /// Returns an HTTP 200 response if the username is available, otherwise an HTTP 400 response.
        /// </returns>
        /// <remarks>
        /// This method checks if the given username exists in the database and returns a success or failure result.
        /// </remarks>
        [HttpPost("verify-username")]
        public async Task<ActionResult> VerifyUsername(string username)
        {
            // Call the account service to check if the username is registered
            var result = await _accountService.VerifyUsername(username); // Corrected method call

            // If the username is available, return a success response
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            // Otherwise, return a failure response
            return BadRequest(result);
        }


        /// <summary>
        /// Registers a new user with the provided information.
        /// </summary>
        /// <param name="registerDTO">The user registration details.</param>
        /// <returns>
        /// - Returns HTTP 200 (OK) if registration is successful.
        /// - Returns HTTP 400 (Bad Request) if validation fails.
        /// - Returns HTTP 409 (Conflict) if the email or username is already in use.
        /// </returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            var result = await _accountService.Register(registerDTO);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            if (result.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
            {
                return Conflict(result);
            }

            return BadRequest(result);
        }



        /// <summary>
        /// Verifies the registration process using an OTP code.
        /// </summary>
        /// <param name="otpCodeDTO">The OTP code details for verification.</param>
        /// <returns>
        /// - Returns an HTTP 200 response with an access token if verification is successful.
        /// - Returns an HTTP 401 (Unauthorized) response if the OTP code is denied.
        /// - Returns an HTTP 400 (Bad Request) response if the user is not found or verification fails.
        /// </returns>
        /// <remarks>
        /// This method:
        /// - Calls the account service to verify the provided OTP code.
        /// - If successful, retrieves the user and generates an access token.
        /// - If the OTP verification fails, appropriate error responses are returned.
        /// </remarks>
        [HttpPost("verify-register")]
        public async Task<ActionResult> VerifyRegister(OtpCodeDTO otpCodeDTO)
        {
            // Attempt to verify the OTP code
            var result = await _accountService.Verify(otpCodeDTO);

            // If verification is successful
            if (result.IsSuccess)
            {
                // Retrieve the user by email
                var user = await _accountService.GetUserAsync(otpCodeDTO.Email);

                // If the user does not exist, return an error response
                if (user == null)
                {
                    return BadRequest(Result.Failure("user not found"));
                }

                // Generate an access token for the verified user
                var token = _tokenService.GenerateAccessToken(user);

                // Generate a refresh token for session management
                await _tokenService.GenerateRefreshTokenAsync(user);

                // Return the generated token and success status
                return Ok(new AuthorizeToken
                {
                    Status = true,
                    Message = result.Message,
                    Token = token
                });
            }
            // If the OTP code needs to be requested again, return an Unauthorized response
            else if (result.Message == "re-request the code again")
            {
                return Unauthorized(Result.Failure("access denied"));
            }

            // Return a failure response if verification fails
            return BadRequest(result);
        }


        /// <summary>
        /// Sends a password reset code to the user's email.
        /// </summary>
        /// <param name="email">The email of the user requesting a password reset.</param>
        /// <returns>
        /// - HTTP 200 (OK) if the code is sent successfully.
        /// - HTTP 400 (Bad Request) if the email is invalid or if the request fails.
        /// - HTTP 404 (Not Found) if the email is not registered.
        /// </returns>
        [HttpPost("send-password-reset-code")]
        public async Task<ActionResult> RequestResetPassword([FromBody] string email)
        {
            var result = await _accountService.SendResetPasswordCode(email);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(result);
            }

            return BadRequest(result);
        }


        /// <summary>
        /// Resets the user's password using the provided reset code.
        /// </summary>
        /// <param name="resetPasswordDto">The DTO containing email, reset code, and new password.</param>
        /// <returns>
        /// - HTTP 200 (OK) if the password is changed successfully and returns a new access token.
        /// - HTTP 400 (Bad Request) if the request is invalid or if the password does not meet security standards.
        /// - HTTP 404 (Not Found) if the user does not exist.
        /// </returns>
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result.Failure("provide a valid information"));
            }
            var result = await _accountService.ChangePassword(resetPasswordDto);

            if (result.IsSuccess)
            {
                var user = await _accountService.GetUserAsync(resetPasswordDto.Email);

                if (user == null)
                {
                    return BadRequest(Result.Failure("user not found"));
                }

                var token = _tokenService.GenerateAccessToken(user);

                await _tokenService.GenerateRefreshTokenAsync(user);

                return Ok(new AuthorizeToken
                {
                    Status = true,
                    Message = result.Message,
                    Token = token
                });
            }

            return BadRequest(result);
        }
    }
}
