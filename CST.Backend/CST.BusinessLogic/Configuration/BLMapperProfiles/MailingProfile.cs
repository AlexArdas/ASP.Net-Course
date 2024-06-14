using AutoMapper;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Enums;
using CST.Common.Models.Messages;

namespace CST.BusinessLogic.Configuration.BLMapperProfiles
{
    public class MailingProfile : Profile
    {
        public MailingProfile()
        {
            CreateMap<IHubMailing, MailingDomainEntity>()
                .ForMember(dest => dest.MailingStatus,
                    opt => opt.MapFrom<MailingStatusResolver, IHubMailingStatus>(src => src.Status))
                .ForMember(dest => dest.Importance,
                    opt => opt.MapFrom(src => Enum.GetName(typeof(Importance), src.Importance)))
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.CreatedByExternalId))
                .ForMember(dest => dest.MailingLocations, opt => opt.Ignore())
                .ForMember(dest => dest.Reports, opt => opt.Ignore())
                .ForMember(dest => dest.Channel, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.MailingsApprovers, opt => opt.Ignore());


            CreateMap<MailingDomainEntity, MailingBriefResponse>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId));

            CreateMap<MailingViewModel, MailingDomainEntity>()
                .ForMember(dest => dest.MailingStatus, opt => opt.MapFrom(src => Enum.GetName(typeof(MailingStatus), src.MailingStatus)))
                .ForMember(dest => dest.Importance, opt => opt.MapFrom(src => Enum.GetName(typeof(Importance), src.Importance)))
                .ForMember(dest => dest.MailingLocations, opt => opt.Ignore())
                .ForMember(dest => dest.Reports, opt => opt.Ignore())
                .ForMember(dest => dest.Channel, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.MailingsApprovers, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<MailingDomainEntity, MailingReportResponse>().IgnoreAllNonExisting();
        }
    }
}
