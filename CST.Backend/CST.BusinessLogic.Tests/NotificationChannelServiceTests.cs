#nullable enable
using AutoFixture;
using CST.BusinessLogic.Services;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Messages;
using CST.Common.Repositories;
using CST.Tests.Common;
using Moq;
using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace CST.BusinessLogic.Tests
{
    public class NotificationChannelServiceTests : AutoMockerTestsBase<NotificationChannelService>
    {
        private readonly Mock<INotificationChannelRepository> _notificationChannelRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly NotificationChannelService _notificationChannelService;

        public NotificationChannelServiceTests()
        {
            _notificationChannelRepository = GetMock<INotificationChannelRepository>();
            _mapper = GetMock<IMapper>();
            _notificationChannelService = GetService<NotificationChannelService>();
        }

        [Fact]
        public async Task ProcessMessageAsync_ShouldCreateNotificationChannel()
        {
            //Arrange
            var IHubNotificationChannelInput = Fixture.Create<IHubNotificationChannel>();
            
            var correspondingNotificationChannelDomainEntity =
                CreateNotificationChannelDomainEntity(IHubNotificationChannelInput);

            var expectedNotificationChannelViewModel =
                CreateNotificationChannelViewModel(IHubNotificationChannelInput);
            
            _mapper.Setup(mapper => mapper.Map<NotificationChannelDomainEntity>(IHubNotificationChannelInput))
                .Returns(correspondingNotificationChannelDomainEntity);
            _mapper.Setup(mapper =>
                    mapper.Map<NotificationChannelViewModel>(correspondingNotificationChannelDomainEntity))
                .Returns(expectedNotificationChannelViewModel);

            _notificationChannelRepository
                .Setup(repo => repo.AddAsync(correspondingNotificationChannelDomainEntity))
                .ReturnsAsync(correspondingNotificationChannelDomainEntity);

            //Act
            var result = await _notificationChannelService.ProcessMessageAsync(IHubNotificationChannelInput);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedNotificationChannelViewModel);
        }

        [Fact]
        public async Task ProcessMessageAsync_ShouldUpdateNotificationChannel()
        {
            //Arrange
            var IHubNotificationChannelInput = Fixture.Create<IHubNotificationChannel>();
            var notificationChannelDomainEntityToUpdate = Fixture.Create<NotificationChannelDomainEntity>();
            IHubNotificationChannelInput.Id = notificationChannelDomainEntityToUpdate.Id;
            
            var newNotificationChannelDomainEntity =
                CreateNotificationChannelDomainEntity(IHubNotificationChannelInput);

            var newNotificationChannelViewModel = 
                CreateNotificationChannelViewModel(IHubNotificationChannelInput);

            _mapper.Setup(mapper => mapper.Map<NotificationChannelDomainEntity>(IHubNotificationChannelInput))
                .Returns(newNotificationChannelDomainEntity);
            _mapper.Setup(mapper => mapper.Map<NotificationChannelViewModel>(newNotificationChannelDomainEntity))
                .Returns(newNotificationChannelViewModel);

            _notificationChannelRepository.Setup(repo => repo.ExistsAsync(It.IsAny<Guid>()).Result).Returns(true);
            _notificationChannelRepository.Setup(repo => repo.GetItemByIdAsync(It.Is<Guid>(id => id == notificationChannelDomainEntityToUpdate.Id)))
                                          .ReturnsAsync(notificationChannelDomainEntityToUpdate);
            _notificationChannelRepository.Setup(repo => repo.UpdateNotificationChannelAsync(newNotificationChannelDomainEntity))
                                          .ReturnsAsync(newNotificationChannelDomainEntity);


            //Act
            var result = await _notificationChannelService.ProcessMessageAsync(IHubNotificationChannelInput);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(newNotificationChannelViewModel);
        }

        private NotificationChannelDomainEntity CreateNotificationChannelDomainEntity(IHubNotificationChannel IHubNotificationChannelInput)
        {
            return Fixture.Build<NotificationChannelDomainEntity>()
                .With(nc => nc.Id, IHubNotificationChannelInput.Id)
                .With(nc => nc.Name, IHubNotificationChannelInput.Name)
                .With(nc => nc.Description, IHubNotificationChannelInput.Description)
                .With(nc => nc.Brief, IHubNotificationChannelInput.Brief)
                .With(nc => nc.IsPrivate, IHubNotificationChannelInput.IsPrivate)
                .With(nc => nc.PersonalBlogScope, IHubNotificationChannelInput.PersonalBlogScope)
                .With(nc => nc.CreatedOn, IHubNotificationChannelInput.CreatedOn)
                .With(nc => nc.DeletedOn, IHubNotificationChannelInput.DeletedOn)
                .With(nc => nc.TeamsLink, IHubNotificationChannelInput.TeamsLink)
                .Create();
        }

        private NotificationChannelViewModel CreateNotificationChannelViewModel(IHubNotificationChannel IHubNotificationChannelInput)
        {
            return Fixture.Build<NotificationChannelViewModel>()
                .With(nc => nc.Id, IHubNotificationChannelInput.Id)
                .With(nc => nc.Name, IHubNotificationChannelInput.Name)
                .With(nc => nc.Description, IHubNotificationChannelInput.Description)
                .With(nc => nc.Brief, IHubNotificationChannelInput.Brief)
                .With(nc => nc.IsPrivate, IHubNotificationChannelInput.IsPrivate)
                .With(nc => nc.PersonalBlogScope, IHubNotificationChannelInput.PersonalBlogScope)
                .With(nc => nc.CreatedOn, IHubNotificationChannelInput.CreatedOn)
                .With(nc => nc.DeletedOn, IHubNotificationChannelInput.DeletedOn)
                .With(nc => nc.TeamsLink, IHubNotificationChannelInput.TeamsLink)
                .Create();
        }
    }
}

