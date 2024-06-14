using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using CST.BusinessLogic.Configuration.BLMapperProfiles;
using CST.BusinessLogic.Services;
using CST.Common.Exceptions;
using CST.Common.Models.Context;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Enums;
using CST.Common.Models.Messages;
using CST.Common.Repositories;
using CST.Common.Services;
using CST.Tests.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace CST.BusinessLogic.Tests
{
    public class MailingServiceTests : AutoMockerTestsBase<MailingService>
    {
        private readonly Mock<INotificationChannelRepository> _notificationChannelRepository;
        private readonly Mock<IMailingRepository> _mailingRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IIHubService> _iHubService;
        private readonly Mock<IMapper> _mapper;
        private readonly MailingService _mailingService;

        public MailingServiceTests()
        {
            _notificationChannelRepository = GetMock<INotificationChannelRepository>();
            _mailingRepository = GetMock<IMailingRepository>();
            _userRepository = GetMock<IUserRepository>();
            _iHubService = GetMock<IIHubService>();
            _mapper = GetMock<IMapper>();
            _mailingService = GetService<MailingService>();

            UseMapperWithProfiles(new MailingProfile());
        }


        [Fact]
        public async Task FilterMailingsAsync_ShouldReturnFilteredListOfMailingFilterResponses()
        {
            //Arrange
            var mailingFilterResponses = Fixture.Create<List<MailingFilterResponse>>();
            var mailingFilterRequest = Fixture.Create<MailingFilterRequest>();

            _mailingRepository.Setup(mr => mr.FilterMailingsAsync(mailingFilterRequest)).ReturnsAsync(mailingFilterResponses);

            //Act
            var result = await Target.FilterMailingsAsync(mailingFilterRequest);

            //Assert
            _mailingRepository.Verify(r => r.FilterMailingsAsync(mailingFilterRequest), Times.Once);
            result.Should().BeOfType(mailingFilterResponses.GetType());
        }

        [Fact]
        public async Task ProcessMessageAsync_ShouldReturnAddedMailingWithAddedChannel()
        {
            //Arrange
            var mailingIHub = Fixture.Create<IHubMailing>();
            var notificationChannelDe = Fixture.Create<NotificationChannelDomainEntity>();
            var mailingDe = Fixture.Create<MailingDomainEntity>();
            var mailingVm = Fixture.Create<MailingViewModel>();
            var users = Fixture.Create<List<UserDomainEntity>>();
            var approversEmails = mailingDe.ChanelApproversEmails.Union(mailingDe.LocationApproversEmails).ToList();
            var appoversIds = users.Select(u => u.Id).ToList();

            _mapper.Setup(m => m.Map<IHubMailing, MailingDomainEntity>(mailingIHub)).Returns(mailingDe);
            _mapper.Setup(m => m.Map<MailingDomainEntity, MailingViewModel>(mailingDe)).Returns(mailingVm);

            _notificationChannelRepository.Setup(nr => nr.ExistsAsync(mailingDe.ChannelId.Value))
                .ReturnsAsync(false);

            _notificationChannelRepository.Setup(nr => nr.CreateDefaultNotificationChannel(mailingDe.ChannelId.Value))
                .Returns(notificationChannelDe);

            _mailingRepository.Setup(mr => mr.ExistsAsync(It.IsAny<Guid>()).Result).Returns(false);
            _mailingRepository.Setup(mr => mr.AddAsync(mailingDe)).ReturnsAsync(mailingDe);

            _userRepository.Setup(ur => ur.GetUsersIdsByEmailsAsync(approversEmails)).ReturnsAsync(appoversIds);

            //Act
            var result = await _mailingService.ProcessMessageAsync(mailingIHub);

            //Assert
            result.Should().BeOfType(mailingVm.GetType());
            _mailingRepository.Verify(mr => mr.ExistsAsync(It.IsAny<Guid>()).Result, Times.Once);
            _mailingRepository.Verify(mr => mr.AddAsync(mailingDe), Times.Once);

        }

        [Fact]
        public async Task ProcessMessageAsync_ShouldReturnUpdatedMailingWithExistingChannel()
        {
            //Arrange
            var mailingIHub = Fixture.Create<IHubMailing>();
            var mailingDe = Fixture.Create<MailingDomainEntity>();
            var mailingVm = Fixture.Create<MailingViewModel>();
            var users = Fixture.Create<List<UserDomainEntity>>();
            var approversEmails = mailingDe.ChanelApproversEmails.Union(mailingDe.LocationApproversEmails).ToList();
            var appoversIds = users.Select(u => u.Id).ToList();

            _mapper.Setup(m => m.Map<IHubMailing, MailingDomainEntity>(mailingIHub)).Returns(mailingDe);
            _mapper.Setup(m => m.Map<MailingDomainEntity, MailingViewModel>(mailingDe)).Returns(mailingVm);

            _notificationChannelRepository.Setup(nr => nr.ExistsAsync(mailingDe.ChannelId.Value))
                .ReturnsAsync(true);

            _mailingRepository.Setup(mr => mr.ExistsAsync(It.IsAny<Guid>()).Result).Returns(true);
            _mailingRepository.Setup(mr => mr.UpdateMailingAsync(mailingDe)).ReturnsAsync(mailingDe);

            _userRepository.Setup(ur => ur.GetUsersIdsByEmailsAsync(approversEmails)).ReturnsAsync(appoversIds);

            //Act
            var result = await _mailingService.ProcessMessageAsync(mailingIHub);

            //Assert
            result.Should().BeOfType(mailingVm.GetType());
            _mailingRepository.Verify(mr => mr.ExistsAsync(It.IsAny<Guid>()).Result, Times.Once);
            _mailingRepository.Verify(mr => mr.UpdateMailingAsync(mailingDe), Times.Once);
        }


        [Fact]
        public async Task GetMailingDescriptionAsync_ShouldReturnMailingDescriptionModel()
        {
            //Arrange
            var mailing = Fixture.Create<MailingDomainEntity>();
            var mailingDescriptionModel = Fixture.Create<MailingDescriptionResponse>();

            _mailingRepository.Setup(mr => mr.GetMailingDescriptionAsync(mailing.Id)).ReturnsAsync(mailingDescriptionModel);

            //Act
            var result = await Target.GetMailingDescriptionAsync(mailing.Id);

            //Assert
            _mailingRepository.Verify(mr => mr.GetMailingDescriptionAsync(mailing.Id), Times.Once);
            result.Should().NotBeNull();
            result.Should().BeOfType(mailingDescriptionModel.GetType());
        }

        [Fact]
        public async Task GetMailingsReportByIdsAsync_ShouldReturnMailingReportResponse()
        {
            //Arrange
            var mailingEntities = Fixture.Create<List<MailingDomainEntity>>();
            var mailingIds = mailingEntities.Select(me => me.Id).ToList();
            var mailingsReport = Fixture.Create<List<MailingReportResponse>>();
            
            _mailingRepository.Setup(mr => mr.GetMailingsReportByIdsAsync(mailingIds)).ReturnsAsync(mailingsReport);

            //Act
            var result = await Target.GetMailingsReportByIdAsync(mailingIds);

            //Assert
            _mailingRepository.Verify(mr => mr.GetMailingsReportByIdsAsync(mailingIds), Times.Once);
            result.Should().BeEquivalentTo(mailingsReport);
        }

        [Fact]
        public async Task CancelMailingAsync_ShouldNotThrow()
        {
            //Arrange
            var mailing = Fixture.Build<MailingDomainEntity>()
                .With(m => m.MailingStatus, MailingStatus.InProgress)
                .With(m => m.SendOn, DateTime.UtcNow.AddDays(1))
                .Create();
            var user = Fixture.Create<UserDomainEntity>();
            
            _mailingRepository.Setup(mr => mr.GetItemByIdAsync(mailing.Id)).ReturnsAsync(mailing);
            
            _userRepository.Setup(mr => mr.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);
            
            _iHubService.Setup(hs => hs.CancelMailingAtIhub(mailing.Id, user.ExternalId))
                .ReturnsAsync(new HttpResponseMessage() { StatusCode = HttpStatusCode.OK });

            //Act
            await Target.CancelMailingAsync(mailing.Id, user.Email);

            //Assert
            mailing.MailingStatus.Should().Be(MailingStatus.Draft);
            _mailingRepository.Verify(mr => mr.GetItemByIdAsync(mailing.Id), Times.Once);
            _userRepository.Verify(mr => mr.GetUserByEmailAsync(user.Email), Times.Once);
            _mailingRepository.Verify(mr => mr.UpdateMailingAsync(mailing), Times.Once);

        }

        [Theory]
        [MemberData(nameof(IncorrectCancelMailings))]
        public async Task CancelMailingAsync_ShouldThrowBadRequestException(MailingDomainEntity incorrectMailing)
        {
            //Arrange
            var mailing = incorrectMailing;
            var user = Fixture.Create<UserDomainEntity>();
            var mail = "test@mail.ru";

            _mailingRepository.Setup(mr => mr.GetItemByIdAsync(mailing.Id)).ReturnsAsync(mailing);

            _userRepository.Setup(mr => mr.GetUserByEmailAsync(mail)).ReturnsAsync(user);

            //Act
            var func = async () => await Target.CancelMailingAsync(mailing.Id, mail);

            //Assert
            await func.Should().ThrowAsync<BadRequestException>();
        }

        public static IEnumerable<object[]> IncorrectCancelMailings =>
            new List<object[]>
            {
                new object[] { new MailingDomainEntity{MailingStatus = MailingStatus.Sent, SendOn = null} },
                new object[] { new MailingDomainEntity{MailingStatus = MailingStatus.Scheduled, SendOn = DateTime.UtcNow.AddDays(-1)} }
            };

        [Fact]
        public async Task CancelMailingAsync_ShouldThrowCstBaseException()
        {
            var mailing = Fixture.Build<MailingDomainEntity>()
                .With(m => m.MailingStatus, MailingStatus.InProgress)
                .With(m => m.SendOn, DateTime.UtcNow.AddDays(1))
                .Create();

            var user = Fixture.Create<UserDomainEntity>();

            _mailingRepository.Setup(mr => mr.GetItemByIdAsync(mailing.Id)).ReturnsAsync(mailing);

            _userRepository.Setup(mr => mr.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);

            _iHubService.Setup(hs => hs.CancelMailingAtIhub(mailing.Id, user.ExternalId))
                .ReturnsAsync(new HttpResponseMessage() { StatusCode = HttpStatusCode.InternalServerError });

            //Act
            var func = async () => await Target.CancelMailingAsync(mailing.Id, user.Email);

            //Assert
            await func.Should().ThrowAsync<CstBaseException>();
        }

        [Fact]
        public async Task GetMailingDaterangeAsync_ShouldReturnDateOfTheEarliestAndNewestMailings()
        {
            //Arrange
            var mailingDaterange = Fixture.Create<MailingDaterange>();
            
            _mailingRepository.Setup(mr => mr.GetMailingDaterangeAsync()).ReturnsAsync(mailingDaterange);
            
            //Act
            var result = await Target.GetMailingDaterangeAsync();

            //Assert
            result.Should().BeOfType(mailingDaterange.GetType());
            _mailingRepository.Verify(mr => mr.GetMailingDaterangeAsync(), Times.Once);
        }

        [Fact]
        public async Task RestoreMailingsAuthorsAsync_ShouldUpdateMailingsAuthors()
        {
            //Arrange
            _mailingRepository.Setup(mr => mr.RestoreMailingsAuthorsAsync());
            
            //Act
            await Target.RestoreMailingsAuthorsAsync();

            //Assert
           _mailingRepository.Verify(mr => mr.RestoreMailingsAuthorsAsync(),Times.Once);
        }
    }
}
