using Resend;

namespace BluntServe.Interfaces
{
    public interface IEmailService
    {
        Task<String> EmailSend(EmailMessage message);
    }
}
