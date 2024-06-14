using AutoMapper;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.DTO.ReportRequest;
using CST.Common.Models.DTO.ReportResponse;

namespace CST.BusinessLogic.Configuration.BLMapperProfiles
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<ReportDomainEntity, ReportBriefViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<ReportDomainEntity, ReportResponse>()
                .ForMember(dest => dest.KeyNumberSet, opt => opt.MapFrom(src => src.KeyNumberSet))
                .ForMember(dest => dest.ReportColumnSet, opt => opt.MapFrom(src => src.ReportColumnSet));

            CreateMap<ReportRequest, ReportDomainEntity>()
                .IgnoreAllNonExisting()
                .ForMember(dest => dest.KeyNumberSet, opt => opt.MapFrom(src => src.KeyNumberSet))
                .ForMember(dest => dest.ReportColumnSet, opt => opt.MapFrom(src => src.ReportColumnSet));

        }
    }
}
