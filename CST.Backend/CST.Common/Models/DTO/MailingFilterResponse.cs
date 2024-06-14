using CST.Common.Models.Enums;

namespace CST.Common.Models.DTO
{
    public class MailingFilterResponse
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public DateTime? SendOn { get; set; }
        public MailingStatus MailingStatus { get; set; }
        public UserBriefResponse Author { get; set; }
        public NotificationChannelBriefResponse Channel { get; set; }
    }
}
