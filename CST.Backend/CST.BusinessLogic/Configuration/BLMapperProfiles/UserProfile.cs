using AutoMapper;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.DTO.ReportRequest;
using CST.Common.Models.DTO.ReportResponse;

namespace CST.BusinessLogic.Configuration.BLMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDomainEntity, UserViewModel>()
                .IgnoreAllNonExisting()
                .ForMember(dest => dest.RoleNames, opt => opt.MapFrom(
                    src => src.UserRoles.Select(r => r.Role.Name).ToList()))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.JobTitle));
        }
    }
}
