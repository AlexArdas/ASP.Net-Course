using System.ComponentModel.DataAnnotations.Schema;

namespace CST.Common.Models.Domain
{
    [Table("UserRole")]
    public class UserRoleDomainEntity
    {
        public Guid RoleId { get; set; }
        public virtual RoleDomainEntity Role { get; set; }

        public Guid UserId { get; set; }
        public virtual UserDomainEntity User { get; set; }
    }
}
