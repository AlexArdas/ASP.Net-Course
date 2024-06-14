#nullable enable
using System.Globalization;
using System.Reflection;
using CST.Common.Attributes;
using CST.Common.Extensions;
using CST.Common.Models.DTO;
using CST.Common.Models.DTO.Report;
using CST.Common.Models.DTO.ReportResponse;
using CST.Common.Models.Enums;
using CST.Common.Services;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace CST.BusinessLogic.Services
{
    public class ReportGeneratorService : IReportGeneratorService
    {
        private readonly ILocationService _locationService;

        public ReportGeneratorService(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<ReportData> FillReportData(ReportResponse reportResponse)
        {
            var reportData = new ReportData();

            FillKeyNumberSetData(reportData, reportResponse);
            reportData.CreateRow();
            await FillReportColumnSetData(reportData, reportResponse);

            return reportData;
        }

        public byte[] ExcelConverter(ReportData reportData)
        {
            var workbook = new XSSFWorkbook();
            var styleSheet = CreateStyleSheet(workbook);
            var excelSheet = workbook.CreateSheet();
            var rowIndex = 0;
            var cellIndex = 0;

            foreach (var row in reportData.Rows)
            {
                var eRow = excelSheet.CreateRow(rowIndex);
                cellIndex = 0;

                bool mergeGroup = false;
                foreach (var cell in row.Cells)
                {
                    if (!mergeGroup && cell.CellStyle == ReportCellStyle.Group)
                    {
                        mergeGroup = true;
                    }
                    var excelCell = eRow.CreateCell(cellIndex);
                    excelCell.SetCellValue(cell.Value);
                    excelCell.CellStyle = styleSheet[cell.CellStyle];
                    cellIndex++;
                }

                if (mergeGroup)
                {
                    excelSheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, cellIndex - 1));
                }
                rowIndex++;
            }

            for (int i = 0; i < cellIndex; i++)
            {
                excelSheet.AutoSizeColumn(i);
            }

            var memoryStream = new MemoryStream();
            workbook.Write(memoryStream, true);
            memoryStream.Position = 0;
            var buffer = memoryStream.ToArray();
            memoryStream.Close();
            return buffer;
        }

        private Dictionary<ReportCellStyle, ICellStyle> CreateStyleSheet(XSSFWorkbook workbook)
        {
            var styleSheet = new Dictionary<ReportCellStyle, ICellStyle>();

            styleSheet[ReportCellStyle.HeaderLeft] = CreateCellStyle(workbook, 10, true, HorizontalAlignment.Left);
            styleSheet[ReportCellStyle.HeaderRight] = CreateCellStyle(workbook, 10, true, HorizontalAlignment.Right);
            styleSheet[ReportCellStyle.CellLeft] = CreateCellStyle(workbook, 12, false, HorizontalAlignment.Left);
            styleSheet[ReportCellStyle.CellRight] = CreateCellStyle(workbook, 12, false, HorizontalAlignment.Right);
            styleSheet[ReportCellStyle.Group] = CreateCellStyle(workbook, 12, true, HorizontalAlignment.Left, IndexedColors.Grey25Percent.Index);
            styleSheet[ReportCellStyle.Default] = CreateCellStyle(workbook, 10, true, HorizontalAlignment.Left);

            return styleSheet;
        }

        private ICellStyle CreateCellStyle(XSSFWorkbook workbook, int fontHeight, bool fontIsBold, HorizontalAlignment horizontalAlignment, short? fillColor = null)
        {
            var style = workbook.CreateCellStyle();
            var font = workbook.CreateFont();
            font.FontHeightInPoints = fontHeight;
            font.IsBold = fontIsBold;
            style.SetFont(font);
            style.Alignment = horizontalAlignment;
            if (fillColor.HasValue)
            {
                style.FillForegroundColor = fillColor.Value;
                style.FillPattern = FillPattern.SolidForeground;
            }
            return style;
        }

        private void FillKeyNumberSetData(ReportData reportData, ReportResponse reportResponse)
        {
            reportData.CreateRow();

            var columnSet = GetReportableProperties(reportResponse.KeyNumberSet);
            reportData.CreateCell("Name", GetHeaderCellStyleByColumn("Name"));
            foreach (var info in columnSet)
            {
                reportData.CreateCell(ColumnTitle(info.Name), GetHeaderCellStyleByColumn(info.Name));
            }

            reportData.CreateRow();
            reportData.CreateCell(reportResponse.Name, ReportCellStyle.CellLeft);
            foreach (var info in columnSet)
            {
                var value = info.Name switch
                {
                    "IncludeMailingsNumber" => reportResponse.Mailings.Count().ToString("D"),
                    "IncludeOpenRate" => reportResponse.Mailings.Average(m => m.OpenRate).ToString("F2"),
                    "IncludeRating" => reportResponse.Mailings.Average(m => m.Rating)?.ToString("F2"),
                    "IncludeReadTime" => reportResponse.Mailings.Average(m => m.ReadTime).ToString("F2"),
                    _ => string.Empty
                };
                reportData.CreateCell(value, GetDataCellStyleByColumn(info.Name));
            }
        }

        private async Task FillReportColumnSetData(ReportData reportData, ReportResponse reportResponse)
        {
            var columnSet = GetReportableProperties(reportResponse.ReportColumnSet);
            reportData.CreateRow();
            foreach (var info in columnSet)
            {
                reportData.CreateCell(ColumnTitle(info.Name), GetHeaderCellStyleByColumn(info.Name));
            }

            var mailings = reportResponse.Mailings;
            var mailingsLocations = await _locationService.GetMailingsLocationNamesAsync(mailings);
            if (mailingsLocations?.Count > 0)
            {
                mailings.ForEach(m => m.LocationNames =
                    mailingsLocations.ContainsKey(m.Id) ? mailingsLocations[m.Id] : string.Empty);
            }

            if (reportResponse.GroupBy.HasValue
                    && reportResponse.GroupBy.Value != ReportGroupByOption.None)
            {
                FillMailingsGroupedData(reportData, reportResponse);
            }
            else
            {
                FillMailingsSortedData(reportData, reportResponse);
            }
        }

        private void FillMailingsGroupedData(ReportData reportData, ReportResponse reportResponse)
        {
            var columnSet = GetReportableProperties(reportResponse.ReportColumnSet);
            var groupPrefix = reportResponse.GroupBy switch
            {
                ReportGroupByOption.Location => "Location: ",
                ReportGroupByOption.NotificationChannel => "Notification Channel: ",
                _ => "Author: "
            };

            var groupedMailings = reportResponse.GroupBy switch
            {
                ReportGroupByOption.Location => reportResponse.Mailings.OrderBy(m => m.LocationNames).GroupBy(m => m.LocationNames),
                ReportGroupByOption.NotificationChannel => reportResponse.Mailings.OrderBy(m => m.ChannelName).GroupBy(m => m.ChannelName),
                _ => reportResponse.Mailings.OrderBy(m => m.AuthorName).GroupBy(m => m.AuthorName)
            };

            foreach (var group in groupedMailings)
            {
                reportData.CreateRow();
                reportData.CreateCell(groupPrefix + group.Key, ReportCellStyle.Group);
                for (int i = 0; i < columnSet.Length - 1; i++)
                {
                    reportData.CreateCell(string.Empty, ReportCellStyle.Group);
                }

                FillMailingsSortedData(reportData, reportResponse, group.ToList());
            }
        }

        private void FillMailingsSortedData(ReportData reportData, ReportResponse reportResponse, List<MailingReportResponse> groupedMailings = null)
        {
            var columnSet = GetReportableProperties(reportResponse.ReportColumnSet);

            var mailings = reportResponse.Mailings;
            if (groupedMailings != null)
            {
                mailings = groupedMailings;
            }

            var sortedMailings = mailings.AsQueryable().SortBy(reportResponse.SortByField, reportResponse.SortOrder)
                .Select(m => m).ToList();

            foreach (var mailing in sortedMailings)
            {
                reportData.CreateRow();
                foreach (var info in columnSet)
                {
                    reportData.CreateCell(GetMailingValue(info.Name, mailing), GetDataCellStyleByColumn(info.Name));
                }
            }
        }
        
        private string GetMailingValue(string name, MailingReportResponse mailing)
        {
            return name switch
            {
                "IncludeName" => mailing.Subject,
                "IncludeNotificationChannel" => mailing.ChannelName,
                "IncludeAuthor" => mailing.AuthorName,
                "IncludeLocation" => mailing.LocationNames,
                "IncludeReadTime" => mailing.ReadTime.ToString("F2"),
                "IncludeReopens" => mailing.Reopens.ToString(),
                "IncludeOpenRate" => mailing.OpenRate.ToString(),
                "IncludeRating" => mailing.Rating?.ToString("F1"),
                "IncludeClicks" => mailing.Clicks.ToString(),
                "IncludeComments" => mailing.Comments.ToString(),
                "IncludeSendDate" => mailing.SendOn?.ToString("dd MMM yyyy a\\t hh:mm tt", CultureInfo.CreateSpecificCulture("en-US")),
                "IncludeEmployees" => mailing.Employees.ToString(),
                _ => string.Empty
            };
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

        private ReportCellStyle GetDataCellStyleByColumn(string name)
        {
            return name switch
            {
                "Name" or
                "IncludeName" or
                "IncludeNotificationChannel" or
                "IncludeAuthor" or
                "IncludeLocation" => ReportCellStyle.CellLeft,

                "IncludeMailingsNumber" or
                "IncludeOpenRate" or
                "IncludeReadTime" or
                "IncludeSendDate" or
                "IncludeReopens" or
                "IncludeRating" or
                "IncludeClicks" or
                "IncludeComments" or
                "IncludeEmployees" => ReportCellStyle.CellRight,

                _ => ReportCellStyle.Default
            };
        }

        private ReportCellStyle GetHeaderCellStyleByColumn(string name)
        {
            return name switch
            {
                "Name" or
                "IncludeName" or
                "IncludeNotificationChannel" or
                "IncludeAuthor" or
                "IncludeLocation" => ReportCellStyle.HeaderLeft,

                "IncludeMailingsNumber" or
                "IncludeOpenRate" or
                "IncludeReadTime" or
                "IncludeSendDate" or
                "IncludeReopens" or
                "IncludeRating" or
                "IncludeClicks" or
                "IncludeComments" or
                "IncludeEmployees" => ReportCellStyle.HeaderRight,

                _ => ReportCellStyle.Default
            };
        }

        private static PropertyInfo[] GetReportableProperties<T>(T obj)
        {
            return obj.GetType().GetProperties()
                                        .Where(p => !Attribute.IsDefined(p, typeof(IsNotReportable))
                                                    && (bool)p.GetValue(obj))
                                        .ToArray();
        }
    }
}