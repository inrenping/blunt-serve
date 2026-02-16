using BluntServe.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resend;

namespace BluntServe.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController
    {

        private readonly IEmailService _emailService;

        [HttpGet]
        [Route("email/send")]
        public async Task<string> EmailSendFixed()
        {
            /*
             * 
             */
            var message = new EmailMessage();
            message.From = "you@domain.com";
            message.To.Add("user@gmail.com");
            message.Subject = "Hello from Controller API";
            message.TextBody = "Email using Resend .NET SDK";

            var resp = await _emailService.EmailSend(message);

            // _logger.LogInformation("Sent email, with Id = {EmailId}", resp.Content);

            return resp.Content.ToString();
        }
    }
}
