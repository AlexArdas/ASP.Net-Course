#nullable enable
using CST.Common.Models.Enums;

namespace CST.Common.Models.DTO
{
    public class MailingDetailsResponse
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public UserBriefResponse Author { get; set; }
        public DateTime? SendOn { get; set; }
        public NotificationChannelBriefResponse Channel { get; set; }
        public MailingStatus MailingStatus { get; set; }
    }
}
