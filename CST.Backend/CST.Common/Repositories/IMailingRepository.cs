using CST.Common.Models.Context;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;

namespace CST.Common.Repositories
{
    public interface IMailingRepository : IHasIdRepository<MailingDomainEntity>
    {
        Task<List<MailingFilterResponse>> FilterMailingsAsync(MailingFilterRequest filterRequest);

        Task<MailingDaterange> GetMailingDaterangeAsync();

        Task<MailingDomainEntity> UpdateMailingAsync(MailingDomainEntity mailing);

        Task<MailingDescriptionResponse> GetMailingDescriptionAsync(Guid mailingId);

        Task<List<MailingReportResponse>> GetMailingsReportByIdsAsync(List<Guid> mailingIds);

        Task RestoreMailingsAuthorsAsync();

        Task RestoreMailingsLocationsAsync();
    }
}