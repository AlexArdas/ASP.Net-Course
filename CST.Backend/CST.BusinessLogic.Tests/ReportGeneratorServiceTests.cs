using AutoFixture;
using CST.BusinessLogic.Services;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.DTO.Report;
using CST.Common.Models.DTO.ReportResponse;
using CST.Common.Services;
using CST.Tests.Common;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CST.Common.Models.Enums;
using Xunit;

namespace CST.BusinessLogic.Tests
{
    public class ReportGeneratorServiceTests : AutoMockerTestsBase<ReportGeneratorService>

    {
        private readonly Mock<ILocationService> _locationService;

        public ReportGeneratorServiceTests()
        {
            _locationService = GetMock<ILocationService>();
        }

        // This test doesn't work inside docker
        [Fact]
        public async Task ExcelConverter_ShouldReturnByteArray()
        {
            //Arrange
            var reportResponse = Fixture.Build<ReportResponse>()
                .With(r=>r.SortOrder, SortOrder.Asc)
                .With(r=>r.SortByField, "Subject")
                .Create();
            var reportData = await Target.FillReportData(reportResponse);

            //Act
            var result = Target.ExcelConverter(reportData);

            //Assert
            result.Should().BeOfType<byte[]>();
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task FillReportData_ShouldReturnRowsWithCells()
        {
            //Arrange
            var reportResponse = Fixture.Build<ReportResponse>()
                .With(r => r.SortOrder, SortOrder.Asc)
                .With(r => r.SortByField, "Subject")
                .Create();

            //Act
            var result = await Target.FillReportData(reportResponse);
            var rows = result.Rows;

            //Assert
            result.Should().BeOfType<ReportData>();
            rows.Count.Should().BeGreaterThan(4);
            rows[0].Cells.Count.Should().BeGreaterThan(1);
            rows[1].Cells.Count.Should().BeGreaterThan(1);
            rows[2].Cells.Count.Should().Be(0);
            rows[3].Cells.Count.Should().BeGreaterThan(1);
            rows[4].Cells.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task CreateReportData_ShouldCreateCorrectColumnsMatchingReport()
        {
            //Arrange
            var reportResponse = Fixture.Build<ReportResponse>()
                .With(r => r.SortOrder, SortOrder.Asc)
                .With(r => r.SortByField, "Subject")
                .Create();
            var reportData = await Target.FillReportData(reportResponse);

            //Act
            var mismatchCount = CheckColumnsReturnMismatchCount(reportData, reportResponse);

            //Assert
            mismatchCount.Should().Be(0);
        }

        [Fact]
        public async Task CreateReportData_ShouldCreateDataWithRowsMatchingReport()
        {
            //Arrange
            var mailings = Fixture.Create<List<MailingReportResponse>>();
            var locations = Fixture.Create<List<LocationDomainEntity>>();

            var locationNames = string.Join(", ", locations.Select(l => l.Name).ToList());

            var mailingsLocations = new Dictionary<Guid, string>();

            foreach (var mailing in mailings)
            {
                mailing.MailingLocations = locations.Select(l => l.Id).ToList();
                var names = locations
                    .Where(l => mailing.MailingLocations.Contains(l.Id))
                    .Select(l => l.Name).ToList();
                mailingsLocations.Add(mailing.Id, string.Join(", ", names));
            }

            _locationService.Setup(ls => ls.GetMailingsLocationNamesAsync(mailings)).ReturnsAsync(mailingsLocations);

            var reportResponse = Fixture.Build<ReportResponse>()
                .With(rr => rr.Mailings, mailings)
                .With(r => r.SortOrder, SortOrder.Asc)
                .With(r => r.SortByField, "Subject")
                .Create();

            var reportData = await Target.FillReportData(reportResponse);

            //Act
            var mismatchCount = CheckRowsReturnMismatchCount(reportData, reportResponse);

            //Assert
            mismatchCount.Should().Be(0);
        }

        private int CheckColumnsReturnMismatchCount(ReportData reportData, ReportResponse reportResponse)
        {
            var keyNumberSet = reportResponse.KeyNumberSet.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(bool) && (bool)p.GetValue(reportResponse.KeyNumberSet))
                .Select(p => p.Name).ToList();

            var reportColumnSet = reportResponse.ReportColumnSet.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(bool) && (bool)p.GetValue(reportResponse.ReportColumnSet))
                .Select(p => p.Name).ToList();


            foreach (ReportRow row in reportData.Rows)
            {
                foreach (ReportCell cell in row.Cells)
                {
                    if (cell.CellStyle == ReportCellStyle.HeaderLeft ||
                        cell.CellStyle == ReportCellStyle.HeaderRight)
                    {
                        keyNumberSet.RemoveAll(k => ColumnTitle(k) == cell.Value);
                        reportColumnSet.RemoveAll(k => ColumnTitle(k) == cell.Value);
                    }
                }
            }

            return keyNumberSet.Count + reportColumnSet.Count;
        }

        private string ColumnTitle(string name)
        {
            return name switch
            {
                "IncludeMailingsNumber" => "Mailings Number",
                "IncludeOpenRate" => "Open Rate",
                "IncludeReadTime" => "Read Time, s",
                "IncludeName" => "Name",
                "IncludeNotificationChannel" => "Notification Channel",
                "IncludeSendDate" => "Send Date",
                "IncludeAuthor" => "Author",
                "IncludeLocation" => "Location",
                "IncludeReopens" => "Reopens",
                "IncludeRating" => "Rating",
                "IncludeClicks" => "Clicks",
                "IncludeComments" => "Comments",
                "IncludeEmployees" => "Employees",
                _ => name
            };
        }

        private int CheckRowsReturnMismatchCount(ReportData reportData, ReportResponse reportResponse)
        {
            var mismatchCount = 0;

            var mailingsSummary = new List<string>() { reportResponse.Name };
            if (reportResponse.KeyNumberSet.IncludeMailingsNumber)
                mailingsSummary.Add(reportResponse.Mailings.Count.ToString());
            if (reportResponse.KeyNumberSet.IncludeOpenRate)
                mailingsSummary.Add(reportResponse.Mailings.Average(m => m.OpenRate).ToString("F2"));
            if (reportResponse.KeyNumberSet.IncludeRating)
                mailingsSummary.Add(reportResponse.Mailings.Average(m => m.Rating)?.ToString("F2"));
            if (reportResponse.KeyNumberSet.IncludeReadTime)
                mailingsSummary.Add(reportResponse.Mailings.Average(m => m.ReadTime).ToString("F2"));

            var mailings = new List<string>();
            if (reportResponse.ReportColumnSet.IncludeName)
                mailings.AddRange(reportResponse.Mailings.Select(m => m.Subject));
            if (reportResponse.ReportColumnSet.IncludeNotificationChannel)
                mailings.AddRange(reportResponse.Mailings.Select(m => m.ChannelName));
            if (reportResponse.ReportColumnSet.IncludeAuthor)
                mailings.AddRange(reportResponse.Mailings.Select(m => m.AuthorName));
            if (reportResponse.ReportColumnSet.IncludeLocation)
                mailings.AddRange(reportResponse.Mailings.Select(m => m.LocationNames));
            if (reportResponse.ReportColumnSet.IncludeEmployees)
                mailings.AddRange(reportResponse.Mailings.Select(m => m.Employees.ToString()));
            if (reportResponse.ReportColumnSet.IncludeReadTime)
                mailings.AddRange(reportResponse.Mailings.Select(m => m.ReadTime.ToString("F2")));
            if (reportResponse.ReportColumnSet.IncludeReopens)
                mailings.AddRange(reportResponse.Mailings.Select(m => m.Reopens.ToString()));
            if (reportResponse.ReportColumnSet.IncludeOpenRate)
                mailings.AddRange(reportResponse.Mailings.Select(m => m.OpenRate.ToString()));
            if (reportResponse.ReportColumnSet.IncludeRating)
                mailings.AddRange(reportResponse.Mailings.Select(m => m.Rating?.ToString("F1")));
            if (reportResponse.ReportColumnSet.IncludeClicks)
                mailings.AddRange(reportResponse.Mailings.Select(m => m.Clicks.ToString()));
            if (reportResponse.ReportColumnSet.IncludeComments)
                mailings.AddRange(reportResponse.Mailings.Select(m => m.Comments.ToString()));
            if (reportResponse.ReportColumnSet.IncludeSendDate)
                mailings.AddRange(reportResponse.Mailings
                    .Select(m => m.SendOn?.ToString("dd MMM yyyy a\\t hh:mm tt",
                                                CultureInfo.CreateSpecificCulture("en-US"))));

            foreach (ReportRow row in reportData.Rows)
            {
                foreach (ReportCell cell in row.Cells)
                {
                    if (cell.CellStyle == ReportCellStyle.CellLeft ||
                        cell.CellStyle == ReportCellStyle.CellRight)
                    {
                        var ms = mailingsSummary.FirstOrDefault(m => m == cell.Value);
                        if (ms != null)
                        {
                            mailingsSummary.Remove(ms);
                        }

                        var mailing = mailings.FirstOrDefault(m => m == cell.Value);
                        if (mailing != null)
                        {
                            mailings.Remove(mailing);
                        }
                    }
                }
            }

            mismatchCount += mailingsSummary.Count;
            mismatchCount += mailings.Count;

            return mismatchCount;
        }
    }
}

