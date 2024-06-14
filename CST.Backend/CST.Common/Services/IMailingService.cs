using CST.Common.Models.Context;
using CST.Common.Models.DTO;
using CST.Common.Models.Messages;

namespace CST.Common.Services
{
    public interface IMailingService
    {
        Task<List<MailingReportResponse>> GetMailingsReportByIdAsync(List<Guid> mailingIds);

        Task<List<MailingFilterResponse>> FilterMailingsAsync(MailingFilterRequest context);

        Task<MailingDaterange> GetMailingDaterangeAsync();

        Task<MailingViewModel> ProcessMessageAsync(IHubMailing mailing);

        Task<MailingDescriptionResponse> GetMailingDescriptionAsync(Guid mailingId);

        Task RestoreMailingsAuthorsAsync();

        Task RestoreMailingsLocationsAsync();


        Task CancelMailingAsync(Guid mailingId, string userEmail);
    }
}
