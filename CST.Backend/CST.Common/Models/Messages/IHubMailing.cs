using CST.Common.Attributes;
using CST.Common.Models.Enums;

namespace CST.Common.Models.Messages
{
    [AzureMessage("mailingqueue")]
    public class IHubMailing : MessageBase
    {
        public Guid Id { get; set; }

        public IHubMailingStatus Status { get; set; }

        public Importance Importance { get; set; }

        public string DistributionGroupEmail { get; set; }

        public string Tags { get; set; }

        public string CreatedByExternalId { get; set; }

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

        public Guid? ChannelId { get; set; }
    }
}
