using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EShopping.Services
{
    public class MyEmailSenderService
        : IEmailSender
    {
        private readonly ILogger<MyEmailSenderService> _logger;

        public MyEmailSenderService(ILogger<MyEmailSenderService> logger)
        {
            _logger = logger;
        }

        #region Microsoft.AspNetCore.Identity.UI.Services.IEmailSender members

        public Task SendEmailAsync(string emailAddress, string subject, string htmlMessage)
        {
            _logger.LogInformation($"Email sent to : {emailAddress}, Content: {htmlMessage}");
            return Task.CompletedTask;
        }

        #endregion
    }
}
