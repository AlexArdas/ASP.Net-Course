using CST.BusinessLogic.Configuration.Sections;
using CST.Common.Services;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Pop3;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace CST.BusinessLogic.Services
{
    public class EmailReceiver : IEmailReceiver
    {
        private readonly Pop3Client _pop3Client;
        private readonly EmailConfiguration _emailConfig;
        private readonly ILogger _logger;


        public EmailReceiver(IConfiguration configuration, ILogger<EmailReceiver> logger)
        {
            _pop3Client = new Pop3Client();
            _emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            _logger = logger;
        }

        public async Task<IList<MimeMessage>> ReceiveMessagesAsync()
        {
            await InitClient();

            var messageCount = await _pop3Client.GetMessageCountAsync();

            if (messageCount == 0)
            {
                return null;
            }

            var messages = await _pop3Client.GetMessagesAsync(0, messageCount);

            return messages;
        }

        public async Task DeleteMessagesAsync(List<string> globalEmailUids)
        {
            await InitClient();

            for (var i = 0; i < await _pop3Client.GetMessageCountAsync(); i++)
            {
                if (globalEmailUids.Contains((await _pop3Client.GetMessageAsync(i)).MessageId))
                {
                    await _pop3Client.DeleteMessageAsync(i);
                }
            }

            await _pop3Client.DisconnectAsync(true);
        }

        private async Task InitClient()
        {
            if (!_pop3Client.IsConnected)
            {
                await _pop3Client.ConnectAsync(_emailConfig.Pop3Host, _emailConfig.Pop3Port);
            }

            if (!_pop3Client.IsAuthenticated)
            {
                await _pop3Client.AuthenticateAsync(_emailConfig.Pop3User, _emailConfig.Pop3Password);
            }

            if (!_pop3Client.IsConnected || !_pop3Client.IsAuthenticated)
            {
                _logger.LogInformation("Pop3 initialization failed.");
            }
        }
    }
}
