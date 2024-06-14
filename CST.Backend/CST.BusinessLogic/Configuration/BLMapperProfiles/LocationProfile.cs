using AutoMapper;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;

namespace CST.BusinessLogic.Configuration.BLMapperProfiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<LocationDomainEntity, LocationViewModel>();
        }
    }
}
