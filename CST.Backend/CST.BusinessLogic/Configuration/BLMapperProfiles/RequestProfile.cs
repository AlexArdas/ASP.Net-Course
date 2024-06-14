using AutoMapper;
using CST.Common.Models.Domain;

namespace CST.BusinessLogic.Configuration.BLMapperProfiles
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<RequestFormDomainEntity, RequestDomainEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.RequestStatus, opt => opt.Ignore())
                .ForMember(dest => dest.RequestFormId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RequestForm, opt => opt.Ignore())
                .ForMember(dest => dest.RequestMessage, opt => opt.Ignore())
                .ForMember(dest => dest.AssigneeId, opt => opt.Ignore())
                .ForMember(dest => dest.Assignee, opt => opt.Ignore())
                .ForMember(dest => dest.RequestReadingsByUser, opt => opt.Ignore());
        }
    }
}
