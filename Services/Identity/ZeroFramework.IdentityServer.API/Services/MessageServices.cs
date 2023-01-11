using System.Net;
using System.Net.Mail;

namespace ZeroFramework.IdentityServer.API.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly IHttpClientFactory _clientFactory;

        private readonly ILogger<AuthMessageSender> _logger;

        public AuthMessageSender(IHttpClientFactory clientFactory, ILogger<AuthMessageSender> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            using SmtpClient client = new("smtp.qq.com")
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("123456@qq.com", "password"),
                Port = 587,
                EnableSsl = true
            };

            MailMessage mailMessage = new()
            {
                From = new MailAddress("123456@qq.com", "水乙方编程")
            };
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            // TODO: Wire this up to actual email sending logic via SendGrid, local SMTP, etc.
            await client.SendMailAsync(mailMessage);
        }

        public async Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            string requestUri = "https://dysmsapi.aliyuncs.com";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var client = _clientFactory.CreateClient("aliyun");

            request.Options.TryAdd("RegionId", "cn-hangzhou");
            request.Options.TryAdd("Version", "2017-05-25");
            request.Options.TryAdd("Action", "SendSms");
            request.Options.TryAdd("PhoneNumbers", number);
            request.Options.TryAdd("SignName", "水乙方");
            request.Options.TryAdd("TemplateParam", new { code = message });
            request.Options.TryAdd("TemplateCode", "SMS_277241031");

            var response = await client.SendAsync(request);

            string result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation(result);
            }
            else
            {
                _logger.LogWarning(result);
            }
        }
    }
}