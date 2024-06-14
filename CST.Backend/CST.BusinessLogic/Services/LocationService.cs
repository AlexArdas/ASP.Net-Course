using AutoMapper;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Repositories;
using CST.Common.Services;

namespace CST.BusinessLogic.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }
        public async Task<List<LocationViewModel>> GetLocationsAsync()
        {
            var entities = await _locationRepository.GetAllAsync();
            return _mapper.Map<List<LocationDomainEntity>, List<LocationViewModel>>(entities);
        }

        public async Task<Dictionary<Guid, string>> GetMailingsLocationNamesAsync(List<MailingReportResponse> mailings)
        {
            return await _locationRepository.GetMailingsLocationNamesAsync(mailings);
        }
    }
}
