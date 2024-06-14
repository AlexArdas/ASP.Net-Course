using AutoMapper;
using CST.Common.Attributes;
using CST.Common.Exceptions;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.DTO.ReportRequest;
using CST.Common.Models.DTO.ReportResponse;
using CST.Common.Repositories;
using CST.Common.Services;
using Microsoft.Extensions.Configuration;


namespace CST.BusinessLogic.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly IReportGeneratorService _reportGeneratorService;
        private readonly IBlobService _blobService;
        private readonly IMailingService _mailingService;
        private readonly string _cloudReportContainerToken;

        public ReportService(IReportRepository reportRepository, IMapper mapper, IReportGeneratorService reportGeneratorService
            , IBlobService blobService, IMailingService mailingService, IConfiguration configuration)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _reportGeneratorService = reportGeneratorService;
            _blobService = blobService;
            _mailingService = mailingService;
            _cloudReportContainerToken = configuration["CloudReportContainerToken"];
        }

        public async Task<List<ReportBriefViewModel>> GetReportsAsync()
        {
            var reportsList = await _reportRepository.GetReportsWithUri();

            reportsList = reportsList.Where(r => IsValidUri(r.Uri))
                .Select(r =>
                {
                    r.Uri = GetReportCloudUri(r.Uri);
                    return r;
                })
                .ToList();

            var reportsBriefList = _mapper.Map<List<ReportDomainEntity>, List<ReportBriefViewModel>>(reportsList);

            return reportsBriefList;
        }

        public async Task<ReportResponse> CreateReportAsync(ReportRequest reportDto)
        {
            var reportResponse = await CreateReportEntityAsync(reportDto);

            await FillInReportResponseMailings(reportResponse);

            await UploadReportToCloudAsync(reportResponse);

            await _reportRepository.UpdateReportSizeAndUriAsync(reportResponse.Id, reportResponse.Uri, reportResponse.FileSize);

            reportResponse.Uri = GetReportCloudUri(reportResponse.Uri);

            return reportResponse;
        }

        public async Task<ReportResponse> CreateReportEntityAsync(ReportRequest reportDto)
        {
            Validate(reportDto);
            var reportEntity = _mapper.Map<ReportDomainEntity>(reportDto);
            await _reportRepository.AddAsync(reportEntity);
            return _mapper.Map<ReportResponse>(reportEntity);
        }

        public async Task UploadReportToCloudAsync(ReportResponse reportResponse)
        {
            var report = await GenerateReportAsync(reportResponse);

            var fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = GetReportName(reportResponse);

            reportResponse.Uri = await _blobService.UploadBlobAsync(fileName, report, fileType);

            reportResponse.FileSize = report.Length;
        }

        public async Task<byte[]> GenerateReportAsync(ReportResponse reportResponse)
        {
            var reportData = await _reportGeneratorService.FillReportData(reportResponse);
            var buffer = _reportGeneratorService.ExcelConverter(reportData);

            return buffer;
        }

        private void Validate(ReportRequest reportDto)
        {
            if (String.IsNullOrEmpty(reportDto.Name))
            {
                throw new InvalidReportException("Field Name is missing or empty");
            }

            if (reportDto.Name.Length > 256)
            {
                throw new InvalidReportException("Report name should be 256 characters max.");
            }

            if (reportDto.MailingIds is null || reportDto.MailingIds.Count == 0)
            {
                throw new InvalidReportException("At least one mailing must be selected.");
            }

            if (reportDto.KeyNumberSet is null)
            {
                throw new InvalidReportException("Report KeyNumberSet should not be null.");
            }

            var noKeyNumberSelected = CheckReportableProperties(reportDto.KeyNumberSet);

            if (noKeyNumberSelected)
            {
                throw new InvalidReportException("At least one out of Key Numbers must be selected.");
            }

            if (reportDto.ReportColumnSet is null)
            {
                throw new InvalidReportException("Report ColumnSet should not be null.");
            }

            var noColumnSelected = CheckReportableProperties(reportDto.ReportColumnSet);

            if (noColumnSelected)
            {
                throw new InvalidReportException("At least one out of Columns must be selected.");
            }

            var availableSortByFields = new[] { "SendOn", "Subject" };

            if (!availableSortByFields.Contains(reportDto.SortByField) && !string.IsNullOrEmpty(reportDto.SortByField))
            {
                throw new InvalidReportException("Report sorting field is incorrect.");
            }
        }

        private static bool CheckReportableProperties<T>(T obj)
        {
            return obj.GetType().GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(IsNotReportable)))
                    .All(p => (bool)p.GetValue(obj) == false);
        }

        private string GetReportName<T>(T model)
        {
            var reportName = model switch
            {
                ReportResponse response => GetReportName(response.Name, response.Id),
                ReportBriefViewModel viewModel => GetReportName(viewModel.Name, viewModel.Id),
                _ => throw new ArgumentOutOfRangeException(nameof(model), model, $"Can't get report name for {model.GetType()} ")
            };

            return reportName;
        }

        private string GetReportName(string name, Guid id)
        {
            return name + id;
        }

        private Uri GetReportCloudUri(Uri reportBlobUri)
        {
           return new Uri(string.Concat(reportBlobUri.ToString(), "?", _cloudReportContainerToken)); 
        }

        private bool IsValidUri(Uri uri)
        {
            return Uri.TryCreate(uri.ToString(), UriKind.Absolute, out _);
        }

        private async Task FillInReportResponseMailings(ReportResponse reportResponse)
        {
            var reportResponseMailings = await _mailingService.GetMailingsReportByIdAsync(reportResponse.MailingIds);

            if (reportResponseMailings is not null && reportResponseMailings.Count > 0)
            {
                reportResponse.Mailings = reportResponseMailings;
            }
            else
            {
                throw new InvalidReportException("At least one existing mailing must be selected.");
            }

            reportResponse.MailingIds = reportResponse.Mailings.Select(rr => rr.Id).ToList();
        }
    }
}

