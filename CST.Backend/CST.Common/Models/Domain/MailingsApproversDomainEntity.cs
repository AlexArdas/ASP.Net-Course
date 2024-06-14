using System.ComponentModel.DataAnnotations.Schema;

namespace CST.Common.Models.Domain
{
    [Table("MailingsApprovers")]
    
    public class MailingsApproversDomainEntity
    {
        public Guid MailingId { get; set; }
        public MailingDomainEntity Mailing { get; set; }
        public Guid ApproverId { get; set; }
        public UserDomainEntity Approver { get; set; }
    }
}
