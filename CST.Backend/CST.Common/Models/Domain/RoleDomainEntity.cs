using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CST.Common.Models.Enums;

namespace CST.Common.Models.Domain
{
    [Table("Role")]
    public class RoleDomainEntity : IHasId
    {
        public Guid Id { get; set; }
        public RoleNames Name { get; set; }
        public string Description { get; set; }
        public virtual List<UserRoleDomainEntity> UserRoles { get; set; }
    }
}
