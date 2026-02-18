using BluntServe.Attributes;
using BluntServe.Interfaces;
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

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpGet("demo")]
        [Log("测试邮件接口")]
        public async Task<string> EmailSendFixed()
        {
            var message = new EmailMessage();
            message.From = "Blunt <onboarding@resend.dev>";
            message.To.Add("inrenping@gmail.com");
            message.Subject = "hello world";
            message.HtmlBody = "<strong>it works!Email using Resend .NET SDK!</strong>";
            var resp = await _emailService.EmailSend(message);
            return resp;
        }
    }
}
