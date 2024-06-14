using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CST.Common.Exceptions;
using CST.Common.Models.Context;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Enums;
using CST.Common.Models.Messages;
using CST.Common.Repositories;
using CST.Common.Services;

namespace CST.BusinessLogic.Services;

public class MailingService : IMailingService
{
    private readonly IMailingRepository _mailingRepository;
    private readonly INotificationChannelRepository _channelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IIHubService _iHubService;
    private readonly IMailingsApproversRepository _mailingsApproversRepository;


    public MailingService(IMailingRepository mailingRepository,
        INotificationChannelRepository channelRepository,
        IUserRepository userRepository,
        IMapper mapper,
        IIHubService iHubService, 
        IMailingsApproversRepository mailingsApproversRepository)
    {
        _mailingRepository = mailingRepository;
        _channelRepository = channelRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _iHubService = iHubService;
        _mailingsApproversRepository = mailingsApproversRepository;
    }

    public async Task<List<MailingReportResponse>> GetMailingsReportByIdAsync(List<Guid> mailingIds)
    {
        return await _mailingRepository.GetMailingsReportByIdsAsync(mailingIds);
    }


    public async Task<List<MailingFilterResponse>> FilterMailingsAsync(MailingFilterRequest filterRequest)
    {
        return await _mailingRepository.FilterMailingsAsync(filterRequest);
    }

    public async Task<MailingDaterange> GetMailingDaterangeAsync()
    {
        return await _mailingRepository.GetMailingDaterangeAsync();
    }

    public async Task<MailingViewModel> ProcessMessageAsync(IHubMailing mailing)
    {
        var mailingDe = _mapper.Map<IHubMailing, MailingDomainEntity>(mailing);

        var cstUser = await _userRepository.GetUserByExternalIdAsync(mailingDe.ExternalId);

        mailingDe.AuthorId = cstUser?.Id;

        if (mailingDe.ChannelId.HasValue &&
            !await _channelRepository.ExistsAsync(mailingDe.ChannelId.Value))
        {
            var notificationChannel = _channelRepository.CreateDefaultNotificationChannel(mailingDe.ChannelId.Value);
            mailingDe.Channel = notificationChannel;
        }

        await UpdateMailingApprovers(mailingDe);

        MailingDomainEntity mailingDb;

        if (!await _mailingRepository.ExistsAsync(mailing.Id))
        {
            mailingDb = await _mailingRepository.AddAsync(mailingDe);
        }
        else
        {
            mailingDb = await _mailingRepository.UpdateMailingAsync(mailingDe);
        }
        return _mapper.Map<MailingDomainEntity, MailingViewModel>(mailingDb);
    }

    public async Task<MailingDescriptionResponse> GetMailingDescriptionAsync(Guid mailingId)
    {
        return await _mailingRepository.GetMailingDescriptionAsync(mailingId);
    }

    public async Task RestoreMailingsAuthorsAsync()
    {
        await _mailingRepository.RestoreMailingsAuthorsAsync();
    }

    public async Task RestoreMailingsLocationsAsync()
    {
        await _mailingRepository.RestoreMailingsLocationsAsync();
    }

    public async Task CancelMailingAsync(Guid mailingId, string userEmail)
    {
        var mailing = await _mailingRepository.GetItemByIdAsync(mailingId);

        var user = await _userRepository.GetUserByEmailAsync(userEmail);

        var validCancelStatus = new[] { MailingStatus.Scheduled, MailingStatus.PendingApproval, MailingStatus.InProgress };

        if (!validCancelStatus.Contains(mailing.MailingStatus))
        {
            throw new BadRequestException("Mailing is not cancelled. Mailing status is not valid");
        }

        if (mailing.SendOn is not null && mailing.SendOn < DateTime.UtcNow)
        {
            throw new BadRequestException("Mailing is not cancelled. Mailing date is not valid");
        }

        var iHubResponse = await _iHubService.CancelMailingAtIhub(mailing.Id, user.ExternalId);

        if (!iHubResponse.IsSuccessStatusCode)
        {
            throw new CstBaseException($"Mailing is not cancelled. IHub returned {iHubResponse.StatusCode}");
        }

        mailing.MailingStatus = MailingStatus.Draft;

        await _mailingRepository.UpdateMailingAsync(mailing);
    }

    private async Task UpdateMailingApprovers(MailingDomainEntity mailingDe)
    {
        var channelApproversEmails = mailingDe.ChanelApproversEmails.Select(x => x.Trim().ToLower()).ToList();

        var locationsApprovesEmails = mailingDe.LocationApproversEmails.Select(x => x.Trim().ToLower()).ToList();

        var approversEmails = channelApproversEmails.Union(locationsApprovesEmails).ToList();

        var approversIds = await _userRepository.GetUsersIdsByEmailsAsync(approversEmails);

        var approversList = approversIds.Select(id => new MailingsApproversDomainEntity()
        {
            MailingId = mailingDe.Id,
            ApproverId = id
        })
            .ToList();

        await _mailingsApproversRepository.ReplaceMailingApprovers(approversList);
    }
}
