using AutoMapper;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO.ReportRequest;
using CST.Common.Models.DTO.ReportResponse;

namespace CST.BusinessLogic.Configuration.BLMapperProfiles
{
    public class KeyNumberSetProfile : Profile
    {
        public KeyNumberSetProfile()
        {
            CreateMap<KeyNumberSetDomainEntity, KeyNumberSetResponse>();
            CreateMap<KeyNumberSetRequest, KeyNumberSetDomainEntity>().IgnoreAllNonExisting();
        }
    }
}
