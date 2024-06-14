using CST.Common.Models.Domain;

namespace CST.Common.Repositories
{
    public interface IMailingsApproversRepository
    {
        Task ReplaceMailingApprovers(List<MailingsApproversDomainEntity> newApprovers);
    }
}
