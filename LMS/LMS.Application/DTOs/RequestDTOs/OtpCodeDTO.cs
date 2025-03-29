using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.RequestDTOs
{
    public class OtpCodeDTO
    {
        [Required(ErrorMessage = "email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "verification code is required")]
        public string Code { get; set; } = string.Empty;
    }
}
