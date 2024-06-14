using System.ComponentModel.DataAnnotations.Schema;

namespace CST.Common.Models.Domain
{
    [Table("ShownRequestToUser")]
    public class ShownRequestToUserDomainEntity
    {
        public Guid UserId { get; set; }
        public virtual UserDomainEntity User { get; set; }
        public Guid RequestId { get; set; }
        public virtual RequestDomainEntity Request { get; set; }
    }

}
