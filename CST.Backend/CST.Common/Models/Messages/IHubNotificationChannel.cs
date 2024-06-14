using CST.Common.Attributes;

namespace CST.Common.Models.Messages
{
    [AzureMessage("notificationchannelqueue")]
    public class IHubNotificationChannel : MessageBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Frequency { get; set; }

        public string Brief { get; set; }

        public bool IsPrivate { get; set; }

        public string Category { get; set; }

        public string PersonalBlogScope { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? LastSynchronizationTime { get; set; }

        public string TeamsLink { get; set; }

        public string ActiveDirectoryGroupEmail { get; set; }

        public string PersonalBlogOwnerEmail { get; set; }

        public List<string> NotificationChannelLocationExternalIds { get; set; }

        public List<string> NotificationChannelApproverEmails { get; set; }
    }
}
