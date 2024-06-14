using AutoMapper;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO.ReportRequest;
using CST.Common.Models.DTO.ReportResponse;

namespace CST.BusinessLogic.Configuration.BLMapperProfiles
{
    public class ReportColumnSetProfile : Profile
    {
        public ReportColumnSetProfile()
        {
            CreateMap<ReportColumnSetDomainEntity, ReportColumnSetResponse>();
            CreateMap<ReportColumnSetRequest, ReportColumnSetDomainEntity>().IgnoreAllNonExisting();
        }
    }
}
