using AutoMapper;
using CST.Common.Models.Domain;
using CST.Common.Models.Enums;
using CST.Common.Models.Messages;

namespace CST.BusinessLogic.Configuration.BLMapperProfiles
{
    internal class MailingStatusResolver : IMemberValueResolver<IHubMailing, MailingDomainEntity, IHubMailingStatus, MailingStatus>
    {
        public MailingStatus Resolve(IHubMailing source, MailingDomainEntity destination, IHubMailingStatus sourceMember, MailingStatus destinationMember, ResolutionContext context)
        {
            return sourceMember switch
            {
                IHubMailingStatus.Submitted => MailingStatus.Scheduled,
                IHubMailingStatus.InProgress => MailingStatus.InProgress,
                
                IHubMailingStatus.PendingChannelApproval
                    or IHubMailingStatus.PendingLocationApproval
                    or IHubMailingStatus.PendingRemarks => MailingStatus.PendingApproval,
                
                IHubMailingStatus.Completed => MailingStatus.Sent,
                IHubMailingStatus.Draft => MailingStatus.Draft,
                IHubMailingStatus.Cancelled => MailingStatus.Cancelled,
                _ => MailingStatus.Draft,
            };
        }
    }
}