using CST.Common.Models.Enums;

namespace CST.Common.Models.DTO
{
    public class UserClaimModel
    {
        public Guid Id { get; set; }

        public List<RoleNames> RoleNames { get; set; }

        public string Email { get; set; }
    }
}



