namespace CST.Common.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Guid messageId, string to, string subject, string html, Action callback = null);
    }
}
