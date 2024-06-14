using CST.Common.Models.Domain;
using MimeKit;

namespace CST.Common.Services
{
    public interface IEmailReceiver
    {
        Task<IList<MimeMessage>> ReceiveMessagesAsync();
        Task DeleteMessagesAsync(List<string> globalEmailUids);
    }
}
