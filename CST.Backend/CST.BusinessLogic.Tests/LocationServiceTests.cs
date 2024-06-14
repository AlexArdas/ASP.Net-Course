using AutoFixture;
using CST.BusinessLogic.Services;
using CST.Tests.Common;
using CST.Common.Models.DTO;
using CST.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using CST.Common.Models.Domain;
using AutoMapper;

namespace CST.BusinessLogic.Tests
{
    public class LocationServiceTests : AutoMockerTestsBase<LocationService>
    {
        private readonly Mock<ILocationRepository> _locationRepository;
        private readonly Mock<IMapper>_mapper;

        public LocationServiceTests()
        {
            _locationRepository = GetMock<ILocationRepository>();
            _mapper = GetMock<IMapper>();
        }

        [Fact]
        public async Task GetLocationsAsync_ShouldReturnListOfLocationDomainEntities()
        {
            //Arrange
            var locationsDomainEntities = Fixture.Create<List<LocationDomainEntity>>();
            _locationRepository.Setup(lr => lr.GetAllAsync()).ReturnsAsync(locationsDomainEntities);
            _mapper.Setup(m => m.Map<List<LocationDomainEntity>, List<LocationViewModel>>(locationsDomainEntities)).Returns(new List<LocationViewModel>());

            //Act
            var result = await Target.GetLocationsAsync();

            //Assert
            result.Should().BeOfType(typeof(List<LocationViewModel>));
            _locationRepository.Verify(lr => lr.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetLocationNamesByIdsAsync_ShouldReturnLocationNamesInString()
        {
            //Arrange
            var mailings = Fixture.Create<List<MailingReportResponse>>();
            var locations = Fixture.Create<List<LocationDomainEntity>>();
            var mailingsLocations = new Dictionary<Guid, string>();
            foreach (var mailing in mailings)
            {
                mailing.MailingLocations = locations.Select(l => l.Id).ToList();
                var names = locations
                    .Where(l => mailing.MailingLocations.Contains(l.Id))
                    .Select(l => l.Name).ToList();
                mailingsLocations.Add(mailing.Id, string.Join(", ", names));
            }
            _locationRepository.Setup(lr => lr.GetMailingsLocationNamesAsync(mailings)).ReturnsAsync(mailingsLocations);

            //Act
            var result = await Target.GetMailingsLocationNamesAsync(mailings);

            //Assert
            _locationRepository.Verify(lr => lr.GetMailingsLocationNamesAsync(mailings), Times.Once);
            result.Should().BeEquivalentTo(mailingsLocations);
        }
    }
}
