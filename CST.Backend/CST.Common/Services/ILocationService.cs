using CST.Common.Models.DTO;

namespace CST.Common.Services
{
    public interface ILocationService
    {
        Task<List<LocationViewModel>> GetLocationsAsync();
        Task<Dictionary<Guid, string>> GetMailingsLocationNamesAsync(List<MailingReportResponse> mailings);
    }
}
