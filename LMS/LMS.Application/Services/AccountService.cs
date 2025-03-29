using AutoMapper;
using LMS.Application.common;
using LMS.Application.DTOs.RequestDTOs;
using LMS.Domain.Entities.Financial;
using LMS.Domain.Entities.Users;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Interfaces;
using LMS.Infrastructure.Specifications;

namespace LMS.Application.Services
{
    public class AccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly CodeService _verifyCodeService;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<LoyaltyLevel> _levelRepository;
        private readonly IMapper _mapper;

        public AccountService(
            IUserRepository userRepository,
            IRepository<Role> roleRepository,
            CodeService verifyCodeService,
            IRepository<Customer> customerRepository,
            IRepository<LoyaltyLevel> levelRespository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _verifyCodeService = verifyCodeService;
            _customerRepository = customerRepository;
            _levelRepository = levelRespository;
            _mapper = mapper;
        }

        /// <summary>
        /// Verifies whether the provided email exists in the system.
        /// </summary>
        /// <param name="email">The email address to be verified.</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating the outcome of the verification:
        /// - If the user is found, it returns a success message.
        /// - If the user is not found, it returns a failure message with details.
        /// </returns>
        /// <remarks>
        /// This method checks if a user exists in the system based on the provided email address. The email comparison is case-insensitive.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown if any unexpected errors occur while checking for the user's existence.
        /// </exception>
        public async Task<Result> VerifyEmail(string email)
        {
            try
            {
                var result = await _userRepository.GetAsync(new Specification<User>(
                    criteria: user => user.Email.ToLower() == email.ToLower()
                    ));

                if (result == null)
                {
                    return Result.Failure("user not found");
                }

                return Result.Success("email is founded");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error : {ex.Message}");
            }
        }

        public async Task<User?> GetUserAsync(string email)
        {
            return await _userRepository.GetAsync(new Specification<User>(
                    criteria: user => user.Email.ToLower() == email.ToLower(),
                    includes: [user => user.Role]
                    ));
        }

        /// <summary>
        /// Verifies whether the provided username exists in the system.
        /// </summary>
        /// <param name="username">The username to be verified.</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating the outcome of the verification:
        /// - If the user is found, it returns a success message.
        /// - If the user is not found, it returns a failure message with details.
        /// </returns>
        /// <remarks>
        /// This method checks if a user exists in the system based on the provided username. The username comparison is case-insensitive.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown if any unexpected errors occur while checking for the user's existence.
        /// </exception>
        public async Task<Result> VerifyUsername(string username)
        {
            try
            {
                var result = await _userRepository.GetAsync(new Specification<User>(
                    criteria: user => user.UserName.ToLower() == username.ToLower()
                    ));

                if (result == null)
                {
                    return Result.Failure("user not found");
                }

                return Result.Success("username is founded");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error : {ex.Message}");
            }
        }



        /// <summary>
        /// Registers a new customer in the system by creating a new user and assigning them a role and loyalty level.
        /// </summary>
        /// <param name="register">The registration data containing the customer's details.</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating the outcome of the registration process:
        /// - If the registration is successful, it returns a result indicating that the verification code has been sent.
        /// - If the customer already exists, it returns a failure message indicating that the user is already registered.
        /// - If any error occurs during the process, it returns a failure message with the error details.
        /// </returns>
        /// <remarks>
        /// This method performs the following steps:
        /// - Checks if a user with the same email or username already exists.
        /// - Retrieves the customer's role (defaulted to "customer") and loyalty level (defaulted to "Bronze").
        /// - Encrypts the password using BCrypt before storing it.
        /// - Adds the customer to the database.
        /// - Sends a verification code to the customer's email address for registration verification.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown if there is any error during the registration process, such as database issues.
        /// </exception>
        public async Task<Result> Register(RegisterDTO register)
        {
            // Map the register request data to a new temporary customer: 
            var newCustomer = _mapper.Map<Customer>(register);

            // Check if this customer already exists:
            var isOldUser = (await _userRepository.GetAsync(new Specification<User>(criteria: (user) =>
                user.Email.ToLower() == register.Email.ToLower() ||
                user.UserName.ToLower() == register.UserName.ToLower()
            ))) != null;

            if (isOldUser)
            {
                return Result.Failure("user already founded");
            }

            // Get the customer's role: 
            var role = await _roleRepository.GetAsync(new Specification<Role>(criteria: role => role.RoleType.ToLower() == "customer".ToLower()));

            if (role == null)
            {
                return Result.Failure("role not found");
            }

            // Get the initial level ==> Bronze level then assign it for the customer:
            var level = await _levelRepository.GetAsync(new Specification<LoyaltyLevel>(criteria: level => level.LevelName.ToLower() == "Bronze".ToLower()));

            if (level == null)
            {
                return Result.Failure("level not found");
            }

            // Link the role with the customer
            newCustomer.RoleId = role.RoleId;

            // Link the level with the customer: 
            newCustomer.LevelId = level.LevelId;

            // Encrypt the password before storing it:
            newCustomer.Password = BCrypt.Net.BCrypt.HashPassword(register.Password);

            // Try to add this customer to the database: 
            try
            {
                await _customerRepository.AddAsync(newCustomer);

                // If adding was successful then send verification code for the provided email with Purpose: Register purpose: 
                var result = await _verifyCodeService.SendCodeAsync(newCustomer.Email, Purpose.RegistrationVerificationTemplate);

                return result;
            }
            // If there is an exception then handle it: 
            catch (Exception ex)
            {
                return Result.Failure($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// Verifies the OTP code provided by the user and performs the necessary actions.
        /// If the OTP code is valid and not expired, the user will be marked as verified.
        /// If the OTP code is expired or maximum attempts are reached, the user will be deleted from the system.
        /// </summary>
        /// <param name="otpCode">The OTP code data transfer object (DTO) containing the email and OTP code entered by the user.</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating the outcome of the OTP verification process:
        /// - If the verification is successful, the user will be marked as verified.
        /// - If the OTP code is invalid, expired, or maximum attempts are reached, the system will delete the user.
        /// - If an error occurs during the process (e.g., database failure), the method will return an error message.
        /// </returns>
        /// <remarks>
        /// This method performs the following steps:
        /// - Verifies the OTP code using the <see cref="IVerifyCodeService"/>.
        /// - If the OTP code is expired or maximum attempts are reached, it deletes the user from the system.
        /// - If the OTP code is valid, it updates the user to mark them as verified.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown if there is any error during the user deletion or update process (e.g., database issues).
        /// </exception>
        public async Task<Result> Verify(OtpCodeDTO otpCode)
        {
            // Verifying the OTP code using the service
            var result = await _verifyCodeService.VerifyOtpCodeAsync(otpCode);

            // Fetch the user from the database based on email
            var user = await _userRepository.GetAsync(new Specification<User>(
                        criteria: us => us.Email.ToLower() == otpCode.Email.ToLower()
                    ));

            if (user == null)
            {
                return result;
            }

            // Check if OTP verification failed and if the failure reason is related to expiration or max attempts
            if (!result.IsSuccess)
            {
                if (
                    result.Message == "This OTP code has expired." ||
                    result.Message == "Maximum attempts reached. The OTP code has been discarded."
                )
                {
                    try
                    {
                        // Delete the user from the system if OTP verification failed due to expiration or max attempts
                        await _userRepository.DeleteHardlyAsync(user!.UserId);
                    }
                    catch (Exception ex)
                    {
                        return Result.Failure($"Error: {ex.Message}");
                    }
                    return Result.Failure("re-request the code again");
                }
                return result;
            }
            else
            {
                // Mark the user as verified if OTP verification succeeded
                user.IsVerified = true;
                try
                {
                    await _userRepository.UpdateAsync(user);
                    return result;
                }
                catch
                {
                    return Result.Failure("can't upgrade this account");
                }
            }
        }

        /// <summary>
        /// Sends a password reset verification code to the specified email.
        /// </summary>
        /// <param name="email">The email address of the user requesting a password reset.</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating the outcome of the operation:
        /// - If the code is sent successfully, a success message is returned.
        /// - If an error occurs, a failure message is returned.
        /// </returns>
        /// <remarks>
        /// This method calls the <see cref="_verifyCodeService.SendCodeAsync"/> service to send a verification code 
        /// using the "PasswordResetTemplate" purpose.
        /// </remarks>
        public async Task<Result> SendResetPasswordCode(string email)
        {
            var result = await _verifyCodeService.SendCodeAsync(email, Purpose.PasswordResetTemplate);

            return result;
        }


        /// <summary>
        /// Changes the user's password after verifying the provided OTP code.
        /// </summary>
        /// <param name="passwordDTO">An object containing the user's email, OTP code, and new password.</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating the outcome of the operation:
        /// - Success if the OTP is valid and the password is updated.
        /// - Failure if the OTP is invalid, expired, or if the user is not found.
        /// - Failure if the new password matches the old one.
        /// - Failure if an error occurs while updating the password.
        /// </returns>
        /// <remarks>
        /// This method first verifies the provided OTP code using <see cref="_verifyCodeService.VerifyOtpCodeAsync"/>.
        /// If the OTP is valid, it ensures the new password is different from the old one.
        /// If valid, the new password is securely hashed and stored in the database.
        /// </remarks>
        public async Task<Result> ChangePassword(ResetPasswordDto passwordDTO)
        {
            // Retrieve the user based on the provided email
            var user = await _userRepository.GetAsync(new Specification<User>(
                criteria: us => us.Email.ToLower() == passwordDTO.Email.ToLower()
            ));

            // Check if the user exists
            if (user == null)
            {
                return Result.Failure("User not found");
            }

            // Get the user's current hashed password
            var oldPassword = user.Password;

            // Check if the new password is the same as the old one
            var IsSamePassword = BCrypt.Net.BCrypt.Verify(passwordDTO.NewPassword, oldPassword);

            if (IsSamePassword)
            {
                return Result.Failure("Use another password"); // Reject if the new password matches the old one
            }

            // Verify the provided OTP code for the email
            var result = await _verifyCodeService.VerifyOtpCodeAsync(new OtpCodeDTO()
            {
                Email = passwordDTO.Email,
                Code = passwordDTO.Code,
            });

            // If OTP verification is successful
            if (result.IsSuccess)
            {
                // Hash the new password before storing it
                user.Password = BCrypt.Net.BCrypt.HashPassword(passwordDTO.NewPassword);

                try
                {
                    // Update the user's password in the database
                    await _userRepository.UpdateAsync(user);
                    return Result.Success("Password updated successfully");
                }
                catch
                {
                    return Result.Failure("Can't update your password"); // Handle update failure
                }
            }
            else
            {
                return Result.Failure(result.Message); // Return OTP verification failure message
            }
        }
    }
}
