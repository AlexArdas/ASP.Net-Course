using AutoMapper;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Messages;

namespace CST.BusinessLogic.Configuration.BLMapperProfiles
{
    public class NotificationChannelProfile : Profile
    {
        public NotificationChannelProfile()
        {
            CreateMap<NotificationChannelViewModel, NotificationChannelDomainEntity>()
               .ForMember(dest => dest.Rank, opt => opt.Ignore())
               .ForMember(dest => dest.MailSubscribers, opt => opt.Ignore())
               .ForMember(dest => dest.TeamsSubscribers, opt => opt.Ignore())
               .ForMember(dest => dest.MailSubscribersCount, opt => opt.Ignore())
               .ForMember(dest => dest.LocationIds, opt => opt.Ignore())
               .ForMember(dest => dest.Approvers, opt => opt.Ignore())
               .ForMember(dest => dest.LocationsTree, opt => opt.Ignore())
               .ForMember(dest => dest.PersonalBlogOwner, opt => opt.Ignore())
               .ForMember(dest => dest.LocationNames, opt => opt.Ignore())
               .ForMember(dest => dest.Mailings, opt => opt.Ignore())
               .ReverseMap(); //Reverse map call might be a bad idea, look into this if bugs arise 

            CreateMap<IHubNotificationChannel, NotificationChannelDomainEntity>()
                .ForMember(dest => dest.Rank, opt => opt.Ignore())
                .ForMember(dest => dest.MailSubscribers, opt => opt.Ignore())
                .ForMember(dest => dest.TeamsSubscribers, opt => opt.Ignore())
                .ForMember(dest => dest.MailSubscribersCount, opt => opt.Ignore())
                .ForMember(dest => dest.LocationIds, opt => opt.Ignore())
                .ForMember(dest => dest.Approvers, opt => opt.Ignore())
                .ForMember(dest => dest.LocationsTree, opt => opt.Ignore())
                .ForMember(dest => dest.PersonalBlogOwner, opt => opt.Ignore())
                .ForMember(dest => dest.LocationNames, opt => opt.Ignore())
                .ForMember(dest => dest.Mailings, opt => opt.Ignore());

            CreateMap<NotificationChannelDomainEntity, NotificationChannelBriefModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));
        }
    }
}
