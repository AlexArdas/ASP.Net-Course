using CST.Common.Exceptions;
using CST.Common.Models.Domain;
using CST.Common.Repositories;
using CST.Dal.SqlContext;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CST.Common.Models.DTO.ReportResponse;

namespace CST.Dal.Repositories
{
    internal class ReportRepository : ItemHasIdRepository<ReportDomainEntity>, IReportRepository
    {
        private readonly IMapper _mapper;

        public ReportRepository(ICstContextFactory dbFactory, IMapper mapper) : base(dbFactory)
        {
            _mapper = mapper;
        }

        public async Task<ReportResponse> GetReportAsync(Guid id)
        {
            var context = DbFactory.CreateContext();
            var domainEntity = await context.ReportDomainEntities
                .Include(x => x.KeyNumberSet)
                .Include(x => x.ReportColumnSet)
                .Include(x => x.Mailings)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (domainEntity is null)
            {
                throw new NotFoundException($"Get Report error. Report {id} not found.");
            }

            return _mapper.Map<ReportDomainEntity, ReportResponse>(domainEntity);
        }

        public async Task<ReportDomainEntity> UpdateReportAsync(ReportDomainEntity report)
        {
            var context = DbFactory.CreateContext();

            var reportDb = await context.ReportDomainEntities.FirstOrDefaultAsync(r => r.Id == report.Id);
            if (reportDb == null)
            {
                throw new NotFoundException($"Update Report error. Report {report.Id} not found.");
            }

            reportDb = report;
            await context.SaveChangesAsync();

            return reportDb;
        }

        public async Task<ReportDomainEntity> UpdateReportSizeAndUriAsync(Guid id, Uri uri, int size)
        {
            var context = DbFactory.CreateContext();

            var reportDb = await context.ReportDomainEntities.FirstOrDefaultAsync(r => r.Id == id);
            if (reportDb == null)
            {
                throw new NotFoundException($"Update Report error. Report {id} not found.");
            }

            reportDb.Uri = uri;
            reportDb.FileSize = size;

            await context.SaveChangesAsync();

            return reportDb;
        }

        public async Task<List<ReportDomainEntity>> GetReportsWithUri()
        {
            var context = DbFactory.CreateContext();

            var reportsWithUri = await context.ReportDomainEntities.Where(r => r.Uri != null).ToListAsync();

            return reportsWithUri;
        }
    }
}
