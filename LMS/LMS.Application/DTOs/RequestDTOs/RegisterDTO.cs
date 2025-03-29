using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.RequestDTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Full is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "provide a valid email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage ="Provide a password with length more than 7-digits")]
        public string Password { get; set; } = string.Empty;
    }
}
