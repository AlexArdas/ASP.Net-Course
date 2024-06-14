using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Enums;

namespace CST.Dal.Extensions
{
    internal static class PermissionFilterExtension
    {
        internal static IQueryable<MailingDomainEntity> FilterForCurrentUser(this IQueryable<MailingDomainEntity> mailings, UserClaimModel currentUser)
        {
            _ = mailings ?? throw new ArgumentNullException(nameof(mailings));
            _ = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            if (currentUser.RoleNames.Contains(RoleNames.CstMccManager)
                && !currentUser.RoleNames.Contains(RoleNames.CstHubAdmin))
            {
                var mailingWithoutApproverStatuses = new List<MailingStatus>
                {
                    MailingStatus.Scheduled, MailingStatus.InProgress, MailingStatus.Sent,
                    MailingStatus.Draft, MailingStatus.Cancelled
                };

                return mailings
                    .Where(m => (mailingWithoutApproverStatuses.Contains(m.MailingStatus) &&
                                 m.AuthorId != currentUser.Id)
                                || m.AuthorId == currentUser.Id
                                || m.MailingsApprovers.Any(ma => ma.ApproverId == currentUser.Id));
            }

            return mailings;
        }
    }
}
