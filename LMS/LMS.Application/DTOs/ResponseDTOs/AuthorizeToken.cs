using LMS.Application.common;

namespace LMS.Application.DTOs.ResponseDTOs
{
    public class AuthorizeToken
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; }
    }
}
