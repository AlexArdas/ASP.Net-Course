namespace CST.Common.Models.DTO
{
    public class UserBriefResponse
    {
        public Guid? Id { get; set; }

        public string? FullName { get; set; }

        public string JobTitle { get; set; }

        public string? AvatarUri { get; set; }
    }
}
