using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.STLDashboardModel;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class BarangayReportController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBarangayReportRepository _repo;
        public BarangayReportController(IConfiguration config, IBarangayReportRepository repo)
        {
            _config = config;
            _repo = repo;
        }


        [HttpPost]
        [Route("reporting/persitio")]
        public async Task<IActionResult> ReportperSitio([FromBody]Report report)
        {
            var result = await _repo.LoadReportperSitio(report);
            if (result.result == Results.Success)
                return Ok(result.household);
            return NotFound();
        }

        [HttpPost]
        [Route("reporting/dataAnalytics")]
        public async Task<IActionResult> DataAnalytics([FromBody] Report report)
        {
            var result = await _repo.DataAnalytics(report);
            if (result.result == Results.Success)
                return Ok(result.household);
            return NotFound();
        }

        [HttpPost]
        [Route("reporting/dynamicReporting")]
        public async Task<IActionResult> DynamicReportData([FromBody] Report report)
        {
            var result = await _repo.DynamicReportData(report);
            if (result.result == Results.Success)
                return Ok(result.household);
            return NotFound();
        }
    }
}
