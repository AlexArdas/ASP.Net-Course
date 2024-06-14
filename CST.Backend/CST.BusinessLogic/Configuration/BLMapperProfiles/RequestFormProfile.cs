using AutoMapper;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;

namespace CST.BusinessLogic.Configuration.BLMapperProfiles
{
    public class RequestFormProfile : Profile
    {
        public RequestFormProfile()
        {
            CreateMap<RequestFormDomainEntity, RequestFormResponse>().IgnoreAllNonExisting();
            CreateMap<RequestFormRequest, RequestFormDomainEntity>().IgnoreAllNonExisting();
            CreateMap<MsFormRequestModel, RequestFormDomainEntity>().IgnoreAllNonExisting();
        }
    }
}
