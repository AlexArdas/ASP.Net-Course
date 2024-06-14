using Microsoft.AspNetCore.Mvc;
using CST.Common.Models.DTO;
using CST.Common.Services;
using CST.Common.Models.DTO.ReportRequest;

namespace CST.Api.Controllers
{
    [ApiController]
    [Route("api/v1/reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Return all mailing reports
        /// </summary>
        /// <returns>List of mailing reports</returns>
        /// <response code="200">List of mailing reports</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ReportBriefViewModel>>> GetReports()
        {
            return Ok(await _reportService.GetReportsAsync());
        }

        /// <summary>
        /// Create report
        /// </summary>
        /// <param name="reportDto">Model to create report from</param>
        /// <response code="400">If input is invalid display error message</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateReport([FromBody] ReportRequest reportDto)
        {
            return Ok(await _reportService.CreateReportAsync(reportDto));
        }
    }
}
