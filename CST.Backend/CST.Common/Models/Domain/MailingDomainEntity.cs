using System.ComponentModel.DataAnnotations.Schema;
using CST.Common.Models.Enums;

namespace CST.Common.Models.Domain
{
    [Table("Mailing")]
    public class MailingDomainEntity : IHasId
    {
        public Guid Id { get; set; }

        public MailingStatus MailingStatus { get; set; }

        public string Importance { get; set; }

        public string DistributionGroupEmail { get; set; }

        public string Tags { get; set; }

        public Guid? AuthorId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? SendOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string Subject { get; set; }

        public List<string> LocationExternalIds { get; set; }

        public int RecipientsCount { get; set; }

        public int DeliveredCount { get; set; }

        public int FailedCount { get; set; }

        public decimal? AverageScore { get; set; }

        public List<Guid> MailingLocations { get; set; }

        public List<string> ChanelApproversEmails { get; set; }

        public List<string> LocationApproversEmails { get; set; }

        public int ReadingTime1To3Seconds { get; set; }

        public int ReadingTime3To9Seconds { get; set; }

        public int ReadingTimeMore9Seconds { get; set; }

        public int LinkClicksCount { get; set; }

        public int FeedbackCommentsCount { get; set; }

        public List<ReportDomainEntity> Reports { get; set; }

        public Guid? ChannelId { get; set; }

        public virtual NotificationChannelDomainEntity Channel { get; set; }
        
        public UserDomainEntity Author { get; set; }

        // From IHub CreatedByExternalId
        public string ExternalId { get; set; }
        public List<MailingsApproversDomainEntity> MailingsApprovers { get; set; }
    }
}
