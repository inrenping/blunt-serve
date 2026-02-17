
using BluntServe.Data;
using BluntServe.Interfaces;
using Resend;
namespace BluntServe.Services
{
    public class EmailService : IEmailService
    {
        private readonly PgDbContext _dbContext;
        private readonly IResend _resend;

        public EmailService(PgDbContext dbContext, IResend resend)
        {
            _dbContext = dbContext;
            _resend = resend;
        }

        public async Task<String> EmailSend(EmailMessage message) {
            var resp = await _resend.EmailSendAsync(message);
            return resp.Content.ToString();
        }
    }
}
