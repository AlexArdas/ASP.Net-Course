using System.ComponentModel.DataAnnotations.Schema;
using CST.Common.Models.Enums;

namespace CST.Common.Models.Domain
{
    [Table("Request")]
    public class RequestDomainEntity : IHasId
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string RequesterEmail { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public Guid? RequestFormId { get; set; }
        [ForeignKey("RequestFormId")]
        public RequestFormDomainEntity RequestForm { get; set; }
        public List<RequestMessageDomainEntity> RequestMessage { get; set; }
        public Guid? AssigneeId { get; set; }
        [ForeignKey(nameof(AssigneeId))]
        public virtual UserDomainEntity Assignee { get; set; }
        public virtual List<ShownRequestToUserDomainEntity> RequestReadingsByUser { get; set; }

    }
}
