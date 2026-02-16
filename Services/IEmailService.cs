using Resend;

namespace BluntServe.Services
{
    public interface IEmailService
    {
        Task<String> EmailSend(EmailMessage message);
    }
}
