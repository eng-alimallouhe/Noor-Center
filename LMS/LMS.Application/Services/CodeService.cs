using LMS.Application.common;
using LMS.Application.DTOs.RequestDTOs;
using LMS.Domain.Entities.Users;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Interfaces;
using LMS.Infrastructure.Specifications;
using System.Security.Cryptography;

namespace LMS.Application.Services
{
    public class CodeService
    {
        private readonly string templatesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<OtpCode> _otpCodeRepository;

        public CodeService(
            IEmailService emailService,
            IUserRepository userRepository,
            IRepository<OtpCode> otpCodeRepository)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _otpCodeRepository = otpCodeRepository;
        }

        /// <summary>
        /// Sends a verification code to the specified user's email for the specified purpose.
        /// </summary>
        /// <param name="email">The email address of the user to send the verification code to.</param>
        /// <param name="type">The purpose of the verification code (e.g., account verification, password reset).</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure.
        /// If successful, returns a success message.
        /// If failed, returns a failure message with details.
        /// </returns>
        /// <remarks>
        /// This method checks if the email exists in the system, generates a new verification code,
        /// and sends it via email using the appropriate HTML template based on the <paramref name="type"/>.
        /// If there is an existing OTP code, it will be deleted and replaced with the new one.
        /// The generated OTP code will be hashed and stored in the repository with an expiration time of 10 minutes.
        /// </remarks>
        public async Task<Result> SendCodeAsync(string email, Purpose type)
        {
            try
            {
                string templatePath = Path.Combine(templatesDirectory, $"{type.ToString()}.html");

                if (!File.Exists(templatePath))
                {
                    return Result.Failure("Template not found.");
                }

                var user = await _userRepository.GetAsync(new Specification<User>(
                    criteria: user => user.Email.ToLower() == email.ToLower(),
                    includes: [user => user.OtpCode]
                    ));


                if (user == null)
                {
                    return Result.Failure("User not found.");
                }

                var otpcode = user.OtpCode;

                if (otpcode != null)
                {
                    await _otpCodeRepository.DeleteHardlyAsync(otpcode.OtpCodeId);
                }

                var username = user.UserName;

                var code = GeneratedCode();

                string emailTemplate = await File.ReadAllTextAsync(templatePath);

                emailTemplate = emailTemplate.Replace("{{name}}", username)
                                              .Replace("{{code}}", code);

                await _emailService.SendEmailAsync(email, "Verify Email", emailTemplate);

                var otpCode = new OtpCode()
                {
                    HashedValue = BCrypt.Net.BCrypt.HashPassword(code),
                    ExpiredAt = DateTime.UtcNow.AddMinutes(10),
                    IsUsed = false,
                    FailedAttempts = 0,
                    UserId = user.UserId,
                    User = user
                };

                await _otpCodeRepository.AddAsync(otpCode);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error: {ex.Message}");
            }

            return Result.Success("Verification code sent to email");
        }


        /// <summary>
        /// Verifies the OTP code provided by the user for email verification or other purposes.
        /// </summary>
        /// <param name="otpCodeDTO">The DTO containing the user's email and the OTP code entered by the user.</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure:
        /// - If successful, returns a success message.
        /// - If failed, returns a failure message with details (e.g., incorrect code, expired code, max attempts reached).
        /// </returns>
        /// <remarks>
        /// This method checks the following conditions:
        /// 1. If the user exists in the system based on the provided email.
        /// 2. If the OTP code exists, has not been used, and has not expired.
        /// 3. If the number of failed attempts is less than 3; otherwise, the OTP is discarded.
        /// 4. If the entered OTP code matches the stored hashed value.
        /// Upon successful verification, the OTP code will be marked as used and deleted.
        /// </remarks>
        public async Task<Result> VerifyOtpCodeAsync(OtpCodeDTO otpCodeDTO)
        {
            string email = otpCodeDTO.Email;

            string code = otpCodeDTO.Code;

            var user = await _userRepository.GetAsync(new Specification<User>(user => user.Email.ToLower() == email.ToLower(),
                                                                             includes: [user => user.OtpCode]));

            if (user == null)
            {
                return Result.Failure("User not found.");
            }

            var otpCode = user.OtpCode;

            if (otpCode == null)
            {
                return Result.Failure("OTP code not found.");
            }

            if (otpCode.IsUsed)
            {
                return Result.Failure("This OTP code has already been used.");
            }


            if (otpCode.ExpiredAt < DateTime.UtcNow)
            {
                await _otpCodeRepository.DeleteHardlyAsync(otpCode.OtpCodeId);
                return Result.Failure("This OTP code has expired.");
            }


            if (otpCode.FailedAttempts >= 3)
            {
                await _otpCodeRepository.DeleteHardlyAsync(otpCode.OtpCodeId);
                return Result.Failure("Maximum attempts reached. The OTP code has been discarded.");
            }


            if (!BCrypt.Net.BCrypt.Verify(code, otpCode.HashedValue))
            {
                otpCode.FailedAttempts += 1;
                var remainingAttempts = 3 - otpCode.FailedAttempts;
                await _otpCodeRepository.UpdateAsync(otpCode);
                return Result.Failure($"Incorrect OTP code. You have {remainingAttempts} attempts remaining.");
            }


            otpCode.IsUsed = true;
            await _otpCodeRepository.UpdateAsync(otpCode);


            await _otpCodeRepository.DeleteHardlyAsync(otpCode.OtpCodeId);

            return Result.Success("OTP code verified successfully.");
        }


        //generate a code with 6-digits:
        private string GeneratedCode()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[6];
                rng.GetBytes(randomBytes);

                int code = BitConverter.ToInt32(randomBytes, 0) % 1000000;
                return Math.Abs(code).ToString("D6");
            }
        }

    }


    public enum Purpose
    {
        AccountVerificationTemplate,
        PasswordResetTemplate,
        RegistrationVerificationTemplate
    }
}
