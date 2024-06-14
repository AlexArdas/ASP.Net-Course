using System.ComponentModel.DataAnnotations.Schema;

namespace CST.Common.Models.Domain
{
    [Table("NotificationChannel")]
    public class NotificationChannelDomainEntity : IHasId
    {
        /// <summary>
        /// This is IHub Id
        /// </summary>
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Rank { get; set; }

        public string Description { get; set; }

        public string Frequency { get; set; }

        public string Brief { get; set; }

        public bool IsPrivate { get; set; }

        public string Category { get; set; }

        public string PersonalBlogScope { get; set; }

        public List<Guid> MailSubscribers { get; set; }

        public List<Guid> TeamsSubscribers { get; set; }

        public int MailSubscribersCount { get; set; }

        public List<Guid> LocationIds { get; set; }

        public List<Guid> Approvers { get; set; }

        public List<Guid> LocationsTree { get; set; }

        public Guid PersonalBlogOwner { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? LastSynchronizationTime { get; set; }
        
        public string TeamsLink { get; set; }

        public string ActiveDirectoryGroupEmail { get; set; }

        public string PersonalBlogOwnerEmail { get; set; }

        public List<string> NotificationChannelLocationExternalIds { get; set; }

        public List<string> NotificationChannelApproverEmails { get; set; }

        public List<string> LocationNames { get; set; }

        public List<MailingDomainEntity> Mailings { get; set; }
    }
}
