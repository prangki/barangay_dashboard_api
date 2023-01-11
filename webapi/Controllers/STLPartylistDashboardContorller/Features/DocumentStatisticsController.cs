using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.STLDashboardModel;



namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class DocumentStatisticsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IDocumentStatisticsRepository _repo;
        public DocumentStatisticsController(IConfiguration config, IDocumentStatisticsRepository repo)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost]
        [Route("docstat/brgyclearance")]
        public async Task<IActionResult> StatBrgyClearance(DocumentStatistics docstat)
        {
            var result = await _repo.GetDocStatBrgyClearance(docstat);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/businessclearance")]
        public async Task<IActionResult> StatBusinessClearance(DocumentStatistics docstat)
        {
            var result = await _repo.GetDocStatBusinessClearance(docstat);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/legaldocs")]
        public async Task<IActionResult> StatLegalDocs(DocumentStatistics docstat)
        {
            var result = await _repo.GetDocStatLegalDocs(docstat);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/deathcertificate")]
        public async Task<IActionResult> StatDeathCertificate(DocumentStatistics docstat)
        {
            var result = await _repo.GetDocStatDeathCertificate(docstat);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/cedula")]
        public async Task<IActionResult> StatCedula(DocumentStatistics docstat)
        {
            var result = await _repo.GetDocStatCedula(docstat);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        //==============status=====================//
        [HttpPost]
        [Route("docstat/showbystatusbrgyclearance")]
        public async Task<IActionResult> ShowByStatusBrgyClearance(int statustype, int datetype, string fromdate, string todate)
        {
            var result = await _repo.GetDocShowByStatusBrgyClearance(statustype, datetype, fromdate, todate);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/showbystatusbusinessclearance")]
        public async Task<IActionResult> ShowByStatusBusinessClearance(int statustype, int datetype, string fromdate, string todate)
        {
            var result = await _repo.GetDocShowByStatusBusinessClearance(statustype, datetype, fromdate, todate);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/showbystatuslegaldocs")]
        public async Task<IActionResult> ShowByStatusLegalDocs(int statustype, int datetype, string fromdate, string todate)
        {
            var result = await _repo.GetDocShowByStatusLegalDocs(statustype, datetype, fromdate, todate);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/showbystatusdeathcertificate")]
        public async Task<IActionResult> ShowByStatusDeathCertificate(int statustype, int datetype, string fromdate, string todate)
        {
            var result = await _repo.GetDocShowByStatusDeathCertificate(statustype, datetype, fromdate, todate);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/showbystatuscedula")]
        public async Task<IActionResult> ShowByStatusCedula(int statustype, int datetype, string fromdate, string todate)
        {
            var result = await _repo.GetDocShowByStatusCedula(statustype, datetype, fromdate, todate);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        //==============MONTHLY Orders=====================//
        [HttpPost]
        [Route("docstat/brgyclearancemonthly02")]
        public async Task<IActionResult> MonthlyStatBrgyClearance02(string year)
        {
            var result = await _repo.GetDocMonthlyOrdersBrgyClearance02(year);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/businessclearancemonthly02")]
        public async Task<IActionResult> MonthlyStatBusinessClearance02(string year)
        {
            var result = await _repo.GetDocMonthlyOrdersBusinessClearance02(year);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/legaldocsmonthly02")]
        public async Task<IActionResult> MonthlyStatLegalDocs(string year)
        {
            var result = await _repo.GetDocMonthlyOrdersLegalDocs02(year);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/deathcertificatemonthly02")]
        public async Task<IActionResult> MonthlyStatDeathCertificate(string year)
        {
            var result = await _repo.GetDocMonthlyOrdersDeathCertificate02(year);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        //==============MONTHLY Revenue=====================//
        [HttpPost]
        [Route("docstat/brgyclearancemonthlyrevenue02")]
        public async Task<IActionResult> MonthlyRevenueBrgyClearance02(string year)
        {
            var result = await _repo.GetDocMonthlyRevenueBrgyClearance02(year);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/businessclearancemonthlyrevenue02")]
        public async Task<IActionResult> MonthlyRevenueBusinessClearance02(string year)
        {
            var result = await _repo.GetDocMonthlyRevenueBusinessClearance02(year);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/legaldocsmonthlyrevenue02")]
        public async Task<IActionResult> MonthlyRevenueLegalDocs02(string year)
        {
            var result = await _repo.GetDocMonthlyRevenueLegalDocs02(year);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/deathcertificatemonthlyrevenue02")]
        public async Task<IActionResult> MonthlyRevenueDeathCertificate02(string year)
        {
            var result = await _repo.GetDocMonthlyRevenueDeathCertificate02(year);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

        [HttpPost]
        [Route("docstat/cedulamonthlyrevenue02")]
        public async Task<IActionResult> MonthlyRevenueCedula02(string year)
        {
            var result = await _repo.GetDocMonthlyRevenueCedula02(year);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }

    }
}
