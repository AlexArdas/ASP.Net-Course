using CST.Common.Models.Enums;

namespace CST.Common.Models.DTO
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public List<RoleNames> RoleNames { get; set; }

        public string Title { get; set; }

        public string AvatarUri { get; set; }
    }
}
