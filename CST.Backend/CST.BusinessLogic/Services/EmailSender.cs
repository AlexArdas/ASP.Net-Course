using CST.BusinessLogic.Configuration.Sections;
using CST.Common.Services;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;


namespace CST.BusinessLogic.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly EmailConfiguration _emailConfig;
        private readonly string MessageIdHeaderField = "messageId";
        private readonly Dictionary<string, Action> _callbackDict = new Dictionary<string, Action>();

        public EmailSender(IConfiguration configuration)
        {
            _smtpClient = new SmtpClient();
            _smtpClient.MessageSent += Smtp_MessageSentCallback;
            _emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        }

        public async Task SendEmailAsync(Guid messageId, string to, string subject, string bodyHtml, Action callback = null)
        {
            if (callback != null)
            {
                _callbackDict.Add(messageId.ToString(), callback);
            }

            await InitClient();

            var message = BuildMessage(messageId, _emailConfig.SmtpUser, to, subject, bodyHtml);

            await _smtpClient.SendAsync(message);
        }

        private void Smtp_MessageSentCallback(object sender, MessageSentEventArgs e)
        {
            if (e.Message.Headers.Contains(MessageIdHeaderField))
            {
                var messageId = e.Message.Headers[MessageIdHeaderField];
                if (_callbackDict.ContainsKey(messageId))
                {
                    _callbackDict[messageId].Invoke();
                }
            }
        }

        private MimeMessage BuildMessage(Guid messageId, string from, string to, string subject, string bodyHtml)
        {
            var message = new MimeMessage();
            message.Headers.Add(MessageIdHeaderField, messageId.ToString());
            message.From.Add(MailboxAddress.Parse(from));
            message.To.Add(MailboxAddress.Parse(to));

            message.Subject = subject;
            var textPart = new TextPart(TextFormat.Html) { Text = bodyHtml };
            message.Body = textPart;

            return message;
        }

        private async Task InitClient()
        {
            if (!_smtpClient.IsConnected)
            {
                await _smtpClient.ConnectAsync(_emailConfig.SmtpHost, _emailConfig.SmtpPort);
            }

            if (!_smtpClient.IsAuthenticated)
            {
                await _smtpClient.AuthenticateAsync(_emailConfig.SmtpUser, _emailConfig.SmtpPassword);
            }
        }
    }
}
