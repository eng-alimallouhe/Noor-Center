using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.RequestDTOs
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "provide a valid email")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Provide a password with length more than 7-digits")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = string.Empty;
    }
}
