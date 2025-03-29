using LMS.Application.common;
using LMS.Application.DTOs.RequestDTOs;
using LMS.Domain.Entities.Users;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Interfaces;
using LMS.Infrastructure.Specifications;
using System.Buffers;

namespace LMS.Application.Services
{
    public class LoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly CodeService _codeService;


        public LoginService(
            IUserRepository userRepository,
            CodeService codeService)
        {
            _userRepository = userRepository;
            _codeService = codeService;
        }

        /// <summary>
        /// Logs in a user using their email and password.
        /// </summary>
        /// <param name="loginDTO">The login request containing email and password.</param>
        /// <returns>
        /// A result indicating success or failure, including account lockout conditions.
        /// </returns>
        /// <remarks>
        /// This method:
        /// - Checks if the user exists and is verified.
        /// - Prevents login if the account is deleted or locked.
        /// - Validates the password and tracks failed attempts.
        /// - Locks the account after 5 failed attempts for 3 days.
        /// </remarks>
        public async Task<Result> LoginWithPassword(LoginDTO loginDTO)
        {
            // Retrieve the user by email (case insensitive)
            var user = await _userRepository.GetAsync(new Specification<User>(
                criteria: user => user.Email.ToLower() == loginDTO.Email.ToLower()
                ));

            if (user == null)
            {
                return Result.Failure("user not found");
            }

            // Check if the user is deleted or not verified
            if (user.IsDeleted)
            {
                return Result.Failure("Account has been deleted");
            }

            if (!user.IsVerified)
            {
                return Result.Failure("Account is not verified. Please verify your email.");
            }

            // Check if the account is locked and still within the lock period
            if (user.IsLocked && user.LockedUntil > DateTime.UtcNow)
            {
                return Result.Failure($"Account is locked until {user.LockedUntil:yyyy-MM-dd HH:mm} UTC");
            }

            // Verify the entered password with the stored hash
            bool isSamePassword = BCrypt.Net.BCrypt.Verify(loginDTO.VerifyMehtodValue, user.Password);

            if (isSamePassword)
            {
                // Reset failed login attempts on successful login
                user.FailedLoginAttempts = 0;
                user.IsLocked = false;
                user.LockedUntil = null;

                try
                {
                    await _userRepository.UpdateAsync(user);
                    return Result.Success("access allowed");
                }
                catch (Exception ex)
                {
                    return Result.Failure($"Error: {ex.Message}");
                }
            }
            else
            {

                user.FailedLoginAttempts += 1;

                if (user.FailedLoginAttempts >= 5)
                {
                    user.IsLocked = true;
                    user.LockedUntil = DateTime.UtcNow.AddDays(3);

                    try
                    {
                        await _userRepository.UpdateAsync(user);
                    }
                    catch (Exception ex)
                    {
                        return Result.Failure($"Error: {ex.Message}");
                    }

                    return Result.Failure($"account locked until {user.LockedUntil.ToString()}");
                }
                else
                {
                    try
                    {
                        await _userRepository.UpdateAsync(user);
                    }
                    catch(Exception ex)
                    {
                        return Result.Failure($"Error: {ex.Message}");
                    }
                }
                    return Result.Failure("password not correct");
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetAsync(new Specification<User>(
                    criteria: user => user.Email.ToLower() == email.ToLower(),
                    includes: [user => user.Role]
                    ));
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            var result = await _userRepository.GetAsync(new Specification<User>(
                criteria: user => user.UserId == userId,
                includes: [user => user.RefreshToken, user => user.Role]
                ));

            return result;
        }
    }
}
