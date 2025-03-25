namespace LMS.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string fromEmail, string subject, string message);
    }
}
