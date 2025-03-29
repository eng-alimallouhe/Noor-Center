using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.RequestDTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "provide a valid email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string VerifyMehtodValue { get; set; } = string.Empty;
    }
}
