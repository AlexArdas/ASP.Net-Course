using CST.Common.Models.Enums;

namespace CST.Common.Models.DTO
{
    public class MailingBriefResponse
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public Guid? AuthorId { get; set; }
        public DateTime? SendOn { get; set; }
        public Guid? ChannelId { get; set; }
        public MailingStatus MailingStatus { get; set; }
    }
}
