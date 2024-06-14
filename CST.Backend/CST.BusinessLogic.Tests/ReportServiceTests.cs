using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using CST.BusinessLogic.Configuration.BLMapperProfiles;
using CST.BusinessLogic.Services;
using CST.Common.Exceptions;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.DTO.ReportRequest;
using CST.Common.Models.DTO.ReportResponse;
using CST.Common.Repositories;
using CST.Common.Services;
using CST.Tests.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace CST.BusinessLogic.Tests
{
    public class ReportServiceTests : AutoMockerTestsBase<ReportService>
    {
        private readonly Mock<IReportRepository> _reportRepository;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<IMailingService> _mailingService;

        public ReportServiceTests()
        {
            _reportRepository = GetMock<IReportRepository>();
            UseMapperWithProfiles(
                new ReportProfile(),
                new KeyNumberSetProfile(),
                new ReportColumnSetProfile());
            _blobService = GetMock<IBlobService>();
            _mailingService = GetMock<IMailingService>();
        }

        [Fact]
        public async Task GetReportsAsync_ShouldReturnListOfReportBriefViewModels()
        {
            //Arrange
            var reports = Fixture.Create<List<ReportDomainEntity>>();
            var reportBriefViewModels = Fixture.Create<List<ReportBriefViewModel>>();
            _reportRepository.Setup(rr => rr.GetReportsWithUri().Result).Returns(reports);

            //Act
            var result = await Target.GetReportsAsync();

            //Assert
            result.Should().BeOfType(reportBriefViewModels.GetType());
            _reportRepository.Verify(rr => rr.GetReportsWithUri().Result, Times.Once);
        }

        [Fact]
        public async Task CreateReportAsync_ShouldUploadAndReturnReportResponse()
        {
            //Arrange
            var reportRequest = Fixture.Build<ReportRequest>()
                .With(rr => rr.SortByField, "Subject")
                .Create();
            var reportEntity = Fixture.Create<ReportDomainEntity>();

            var reportMailings = new List<MailingReportResponse>()
            {
                new() {Id = reportRequest.MailingIds[0]},
                new() {Id = reportRequest.MailingIds[1]},
                new() {Id = reportRequest.MailingIds[2]}
            };

            _reportRepository.Setup(rr => rr.AddAsync(It.IsAny<ReportDomainEntity>())).ReturnsAsync(reportEntity).Verifiable();

            var reportResponse = await Target.CreateReportEntityAsync(reportRequest);
            reportResponse.Mailings = reportMailings;
            var content = new byte[It.IsAny<int>()];

            _blobService.Setup(bs => bs.UploadBlobAsync(It.IsAny<string>(), content
                , It.IsAny<string>())).ReturnsAsync(new Uri("http://test.com"));


            _mailingService.Setup(ms => ms.GetMailingsReportByIdAsync(reportResponse.MailingIds))
                                .ReturnsAsync(reportResponse.Mailings);

            //Act
            var result = await Target.CreateReportAsync(reportRequest);

            //Assert
            result.Should().BeOfType(reportResponse.GetType());
            result.FileSize.Should().Be(content.Length);
            _reportRepository.Verify();
        }

        [Fact]
        public async Task CreateReportEntityAsync_ShouldReturnReportResponseWithCorrectFilledInProperties()
        {
            //Arrange
            var reportRequest = Fixture.Build<ReportRequest>()
                .With(rr => rr.SortByField, "Subject")
                .Create();
            var reportEntity = Fixture.Create<ReportDomainEntity>();
            var reportResponse = Fixture.Create<ReportResponse>();

            _reportRepository.Setup(rr => rr.AddAsync(It.IsAny<ReportDomainEntity>())).ReturnsAsync(reportEntity).Verifiable();

            //Act

            var result = await Target.CreateReportEntityAsync(reportRequest);

            //Assert
            result.Should().BeOfType(reportResponse.GetType());
            _reportRepository.Verify();
        }


        [Theory]
        [MemberData(nameof(IncorrectReports))]
        public async Task CreateReportEntityAsync_ShouldThrowInvalidReportException(ReportRequest incorrectReportRequest)
        {
            //Arrange
            var reportRequest = incorrectReportRequest;

            //Assert
            var func = async () => await Target.CreateReportEntityAsync(reportRequest);


            await func.Should().ThrowAsync<InvalidReportException>();
        }

        public static IEnumerable<object[]> IncorrectReports =>
            new List<object[]>
            {
                new object[] { new ReportRequest{Name = null }},
                new object[] { new ReportRequest{Name = new string('a', 257)}},
                new object[] { new ReportRequest{MailingIds = null, Name = new string('a', 10) } },
                new object[] { new ReportRequest{KeyNumberSet = null, MailingIds = new List<Guid>(), Name = new string('a', 10) } },
                new object[] { new ReportRequest{KeyNumberSet = new KeyNumberSetRequest(), MailingIds = new List<Guid>(), Name = new string('a', 10) } },
                new object[] { new ReportRequest{ReportColumnSet = null, KeyNumberSet = new KeyNumberSetRequest(){IncludeRating = true}, MailingIds = new List<Guid>(), Name = new string('a', 10) }},
                new object[] { new ReportRequest{ReportColumnSet = new ReportColumnSetRequest(), KeyNumberSet = new KeyNumberSetRequest() { IncludeRating = true }, MailingIds = new List<Guid>(), Name = new string('a', 10) } }
            };

        [Fact]
        public async Task GenerateReportAsync_ShouldReturnByteArrayAndReportId()
        {
            //Arrange
            var reportResponse = Fixture.Create<ReportResponse>();

            //Act
            var result = await Target.GenerateReportAsync(reportResponse);

            //Assert
            result.Should().BeOfType<byte[]>();
            _reportRepository.Verify();
        }
    }
}