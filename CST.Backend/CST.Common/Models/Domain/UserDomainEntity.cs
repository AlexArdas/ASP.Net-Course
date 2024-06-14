using CST.Common.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CST.Common.Models.Domain
{
    [Table("User")]
    public class UserDomainEntity : IHasId
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string NativeName { get; set; }
        public Guid LocationId { get; set; }
        public virtual LocationDomainEntity Location { get; set; }
        public DateTime? DoB { get; set; }
        public string JobTitle { get; set; }
        public Gender? Gender { get; set; }
        public string AvatarUri { get; set; }
        public DateTime? MccOpenedAt { get; set; }
        public string JobLevel { get; set; }
        public string JobFunctionBase { get; set; }
        public string PrimarySkill { get; set; }
        public DateTime? FireDate { get; set; }
        public Guid? ManagerId { get; set; }
        public string ExternalId { get; set; }
        public int? TimeZone { get; set; }
        [JsonIgnore, ForeignKey("ManagerId")]
        public virtual UserDomainEntity Manager { get; set; }
        public virtual List<UserRoleDomainEntity> UserRoles { get; set; }
        public List<MailingDomainEntity> Mailings { get; set; }
        public virtual List<ShownRequestToUserDomainEntity> ShownRequestsToUser { get; set; }
        public List<MailingsApproversDomainEntity> MailingApprovers { get; set; }
    }
}
