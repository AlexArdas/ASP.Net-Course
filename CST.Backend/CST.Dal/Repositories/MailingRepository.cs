using CST.Common.Exceptions;
using CST.Common.Models.Context;
using CST.Common.Models.Domain;
using CST.Common.Repositories;
using CST.Dal.SqlContext;
using Microsoft.EntityFrameworkCore;
using CST.Common.Models.DTO;
using CST.Common.Models.Enums;
using CST.Common.Providers;
using CST.Dal.Extensions;

namespace CST.Dal.Repositories
{
    internal class MailingRepository : ItemHasIdRepository<MailingDomainEntity>, IMailingRepository
    {
        private IQueryable<MailingDomainEntity> SearchOptionFilter { get; set; }
        private readonly IDateTimeProvider _dateTimeProvider;

        public MailingRepository(ICstContextFactory dbFactory, IDateTimeProvider dateTimeProvider) : base(dbFactory)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<List<MailingFilterResponse>> FilterMailingsAsync(MailingFilterRequest filterRequest)
        {
            var context = DbFactory.CreateContext();
            var mailingsQuery = context.MailingDomainEntities.AsQueryable();

            SearchOptionFilter = null;

            if (!string.IsNullOrWhiteSpace(filterRequest.AuthorSearchOption))
            {
                FilterBySearchOption(mailingsQuery, filterRequest.AuthorSearchOption, nameof(filterRequest.AuthorSearchOption));
            }

            if (!string.IsNullOrWhiteSpace(filterRequest.ChannelSearchOption))
            {
                FilterBySearchOption(mailingsQuery, filterRequest.ChannelSearchOption, nameof(filterRequest.ChannelSearchOption));
            }

            if (!string.IsNullOrWhiteSpace(filterRequest.SubjectSearchOption))
            {
                FilterBySearchOption(mailingsQuery, filterRequest.SubjectSearchOption, nameof(filterRequest.SubjectSearchOption));
            }

            if (SearchOptionFilter != null)
            {
                mailingsQuery = SearchOptionFilter;
            }

            var currentDateTime = _dateTimeProvider.GetCurrent();

            var addMailingsWithoutSendOn = HasToAddMailingsWithoutSendOn(filterRequest.SendOnAfter, filterRequest.SendOnBefore, currentDateTime);

            mailingsQuery = FilterByDateRange(mailingsQuery, filterRequest.SendOnAfter, filterRequest.SendOnBefore, addMailingsWithoutSendOn);

            if (filterRequest.MailingLocations is { Count: > 0 })
            {
                mailingsQuery = mailingsQuery.Where(m => context.LocationDomainEntities.Any(
                        l =>
                            filterRequest.MailingLocations.Contains(l.Id)
                            && m.LocationExternalIds.Contains(l.ExternalId)));
            }

            if (filterRequest.IncludedMailingStatuses is { Count: > 0 })
            {
                mailingsQuery = mailingsQuery.Where(m => filterRequest.IncludedMailingStatuses.Contains(m.MailingStatus));
            }

            if (filterRequest.ExcludedMailingStatuses is { Count: > 0 })
            {
                mailingsQuery = mailingsQuery.Where(m => !filterRequest.ExcludedMailingStatuses.Contains(m.MailingStatus));
            }

            mailingsQuery = FilterMailingsWithoutAuthor(mailingsQuery);

            return await mailingsQuery.OrderBy(m => m.SendOn).Select(m => new MailingFilterResponse()
            {
                Id = m.Id,
                SendOn = SetSendOn(addMailingsWithoutSendOn, m.SendOn, currentDateTime),
                Subject = m.Subject,
                MailingStatus = m.MailingStatus,
                Author = m.Author != null ? new UserBriefResponse
                {
                    Id = m.Author.Id,
                    FullName = m.Author.FullName,
                    JobTitle = m.Author.JobTitle,
                    AvatarUri = m.Author.AvatarUri
                } : null,
                Channel = m.Channel != null ? new NotificationChannelBriefResponse
                {
                    Id = m.Channel.Id,
                    Title = m.Channel.Name
                } : null,
            }).ToListAsync();
        }

        public async Task<MailingDaterange> GetMailingDaterangeAsync()
        {
            var context = DbFactory.CreateContext();

            if (!context.MailingDomainEntities.Any(m => m.SendOn.HasValue))
            {
                throw new NotFoundException("Sent mailings were not found.");
            }

            var daterange = new MailingDaterange()
            {
                startDate = await context.MailingDomainEntities.MinAsync(m => m.SendOn.Value),
                endDate = await context.MailingDomainEntities.MaxAsync(m => m.SendOn.Value)
            };

            return daterange;
        }

        public async Task<MailingDomainEntity> UpdateMailingAsync(MailingDomainEntity mailing)
        {
            var context = DbFactory.CreateContext();
            context.Update(mailing);
            await context.SaveChangesAsync();

            return mailing;
        }

        public async Task<MailingDescriptionResponse> GetMailingDescriptionAsync(Guid mailingId)
        {
            var mailingsQuery = DbFactory.CreateContext().MailingDomainEntities.AsQueryable();

            mailingsQuery = mailingsQuery.Where(m => m.Id.Equals(mailingId));

            var mailingDescription = await mailingsQuery.Select(m => new MailingDescriptionResponse
            {
                Id = m.Id,
                Subject = m.Subject,
                SendOn = m.SendOn,
                Author = m.AuthorId != null
                            ? new UserBriefResponse
                            {
                                Id = m.Author.Id,
                                FullName = m.Author.FullName,
                                JobTitle = m.Author.JobTitle,
                                AvatarUri = m.Author.AvatarUri
                            }
                            : null,
                NotificationChannel = m.Channel != null
                            ? new NotificationChannelBriefResponse
                            {
                                Id = m.Channel.Id,
                                Title = m.Channel.Name
                            }
                            : null,
                MailingStatus = m.MailingStatus
            }).FirstOrDefaultAsync();

            if (mailingDescription is null)
            {
                throw new NotFoundException($"Mailing with Id {mailingId} was not found.");
            }

            return mailingDescription;
        }

        public async Task<List<MailingReportResponse>> GetMailingsReportByIdsAsync(List<Guid> mailingIds)
        {
            var query = DbFactory.CreateContext()
                .MailingDomainEntities
                .Where(m => mailingIds.Contains(m.Id));
            query = FilterNotCancelledOrDraft(query);

            var mailingsReportResponse = await query
                .Select(m => new MailingReportResponse()
                {
                    Id = m.Id,
                    Subject = m.Subject,
                    ChannelName = m.Channel.Name,
                    AuthorName = m.Author.FullName,
                    MailingLocations = m.MailingLocations,
                    Employees = m.RecipientsCount,
                    ReadTime = 0, 
                    Reopens = 0, 
                    OpenRate = 0,
                    Rating = m.AverageScore,
                    Clicks = m.LinkClicksCount,
                    Comments = m.FeedbackCommentsCount,
                    SendOn = m.SendOn
                })
                .ToListAsync();

            return mailingsReportResponse;
        }

        public async Task RestoreMailingsAuthorsAsync()
        {
            var context = DbFactory.CreateContext();
            var mailingsQuery = context.MailingDomainEntities.AsQueryable();
            var usersQuery = context.UserDomainEntities.AsQueryable();

            var mailings = await mailingsQuery.Where(m => !m.AuthorId.HasValue).ToListAsync();
            var userExternalIds = mailings.Select(m => m.ExternalId).ToList();
            var users = await usersQuery.Where(u => userExternalIds.Contains(u.ExternalId)).ToListAsync();

            foreach (var mailing in mailings)
            {
                foreach (var user in users.Where(user => mailing.ExternalId.Equals(user.ExternalId)))
                {
                    mailing.AuthorId = user.Id;
                    await UpdateMailingAsync(mailing);
                }
            }
        }

        public async Task RestoreMailingsLocationsAsync()
        {
            var context = DbFactory.CreateContext();
            var mailingsQuery = context.MailingDomainEntities.AsQueryable();
            var locationsQuery = context.LocationDomainEntities.AsQueryable();

            var mailings = await mailingsQuery.Where(m => (m.MailingLocations == null || m.MailingLocations.Count == 0)
                                                  && m.LocationExternalIds != null && m.LocationExternalIds.Count != 0)
                .ToListAsync();
            var locationExternalIds = mailings.SelectMany(m => m.LocationExternalIds).Distinct().ToList();
            var locations = await locationsQuery.Where(l => locationExternalIds.Contains(l.ExternalId)).ToListAsync();

            foreach (var mailing in mailings)
            {
                foreach (var locationExternalId in mailing.LocationExternalIds)
                {
                    var location = locations.FirstOrDefault(l => l.ExternalId == locationExternalId);
                    if (location != null)
                    {
                        if (mailing.MailingLocations == null)
                        {
                            mailing.MailingLocations = new List<Guid>();
                        }
                        mailing.MailingLocations.Add(location.Id);
                    }
                }
                if (mailing.MailingLocations != null && mailing.MailingLocations.Count > 0)
                {
                    await UpdateMailingAsync(mailing);
                }
            }
        }

        private void FilterBySearchOption(IQueryable<MailingDomainEntity> query, string searchOption, string optionName)
        {
            query = optionName switch
            {
                "AuthorSearchOption" => query.Where(m => m.Author.FullName.ToLower().Contains(searchOption.ToLower())),
                "SubjectSearchOption" => query.Where(m => m.Subject.ToLower().Contains(searchOption.ToLower())),
                "ChannelSearchOption" => query.Where(m => m.Channel.Name.ToLower().Contains(searchOption.ToLower())),
                _ => throw new ArgumentOutOfRangeException(nameof(optionName), optionName, null)
            };
            SearchOptionFilter = SearchOptionFilter != null ? SearchOptionFilter.Union(query) : query;
        }

        private IQueryable<MailingDomainEntity> FilterNotCancelledOrDraft(IQueryable<MailingDomainEntity> query)
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));
            var mailingStatuses = new List<MailingStatus> { MailingStatus.Cancelled, MailingStatus.Draft };

            return query.Where(m => !mailingStatuses.Contains(m.MailingStatus));
        }

        private IQueryable<MailingDomainEntity> FilterMailingsWithoutAuthor(IQueryable<MailingDomainEntity> query)
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));
            return query.Where(m => m.AuthorId.HasValue);
        }

        private IQueryable<MailingDomainEntity> FilterByDateRange(IQueryable<MailingDomainEntity> query, DateTime? sendOnAfter, DateTime? sendOnBefore, bool addMailingsWithoutSendOn)
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));

            if (!addMailingsWithoutSendOn)
            {
                query = query.Where(m => m.SendOn.HasValue);
            }

            if (sendOnAfter.HasValue)
            {
                query = query.Where(m => m.SendOn >= sendOnAfter || (!m.SendOn.HasValue && addMailingsWithoutSendOn));
            }

            if (sendOnBefore.HasValue)
            {
                query = query.Where(m => m.SendOn <= sendOnBefore || (!m.SendOn.HasValue && addMailingsWithoutSendOn));
            }

            return query;
        }

        private bool HasToAddMailingsWithoutSendOn(DateTime? start, DateTime? end, DateTime currentDateTime)
        {
            if (!start.HasValue && !end.HasValue)
            {
                return true;
            }

            if (!start.HasValue || !end.HasValue)
            {
                return start <= currentDateTime || currentDateTime <= end;
            }

            return start <= currentDateTime && currentDateTime <= end;
        }

        private static DateTime? SetSendOn(bool inRange, DateTime? sendOn, DateTime currentDateTime)
        {
            return (inRange && !sendOn.HasValue) ? currentDateTime : sendOn;
        }
    }
}
