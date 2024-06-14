using CST.Common.Models.Enums;

namespace CST.Common.Models.DTO
{
    public class UserResponse
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public List<RoleNames> RoleNames { get; set; }

        public string JobTitle { get; set; }

        public string AvatarUri { get; set; }
    }
}
