using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.STLDashboardModel;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class ReportingController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IReportingRepository _repo;
        private readonly IAccountRepository _loginrepo;
        public ReportingController(IConfiguration config, IReportingRepository repo, IAccountRepository loginrepo)
        {
            _config = config;
            _repo = repo;
            _loginrepo = loginrepo;
        }
        [HttpPost]
        [Route("accountstatistic")]
        public async Task<IActionResult> Statistic(AcctStatistic acct)
        {
            var result = await _repo.AcctStatistic(acct);
            if (result.result == Results.Success)
                return Ok(result.acctstatic);
            return NotFound();
        }
        [HttpPost]
        [Route("accountstatisticmember")]
        public async Task<IActionResult> StatisticMember(AcctStatistic acct)
        {
            var result = await _repo.AcctStatisticMember(acct);
            if (result.result == Results.Success)
                return Ok(result.acctm);
            return NotFound();
        }
        [HttpPost]
        [Route("accountstatisticleader")]
        public async Task<IActionResult> StatisticLeader(AcctStatistic acct)
        {
            var result = await _repo.AcctStatisticLeader(acct);
            if (result.result == Results.Success)
                return Ok(result.acctl);
            return NotFound();
        }
        [HttpPost]
        [Route("donationreport")]
        public async Task<IActionResult> Donation(AcctStatistic acct)
        {
            var result = await _repo.DonnationReport(acct);
            if (result.result == Results.Success)
                return Ok(result.donation);
            return NotFound();
        }

        [HttpPost]
        [Route("employeereport")]
        public async Task<IActionResult> EmployeeReport(AcctStatistic acct)
        {
            var result = await _repo.EmployeeReport(acct);
            if (result.result == Results.Success)
                return Ok(result.donation);
            return NotFound();
        }
        [HttpPost]
        [Route("donationreportleadermember")]
        public async Task<IActionResult> DonationLeaderMember(AcctStatistic acct)
        {
            var result = await _repo.DonationReportLeaderMember(acct);
            if (result.result == Results.Success)
                return Ok(result.donation);
            return NotFound();
        }
    }
}
