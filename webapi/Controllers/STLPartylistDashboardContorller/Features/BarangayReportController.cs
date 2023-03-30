using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto;
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
    public class BarangayReportController : ControllerBase
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
        public async Task<IActionResult> ReportperSitio([FromBody] Report report)
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
        [Route("reporting/statisticalData")]
        public async Task<IActionResult> GetStatistics([FromBody] StatisticalData data)
        {
            var result = await _repo.GetStatisticalData(data);
            if (result.result == Results.Success)
                return Ok(result.household);
            return NotFound();
        }

        [HttpPost]
        [Route("reporting/statisticalData02")]
        public async Task<IActionResult> GetStatistics02([FromBody] StatisticalData data)
        {
            var result = await _repo.GetStatisticalData02(data);
            if (result.result == Results.Success)
                return Ok(result.household);
            return NotFound();
        }

        [HttpPost]
        [Route("reporting/statisticspopulation01")]
        public async Task<IActionResult> Statisticspopulation01(string xml)
        {
            var result = await _repo.GetStatistics0100(xml);
            if (result.result == Results.Success)
                return Ok(result.statistics);
            return NotFound();
        }

        [HttpPost]
        [Route("reporting/statisticspopulation02")]
        public async Task<IActionResult> Statisticspopulation02(int type, string code)
        {
            var result = await _repo.GetStatistics0200(type, code);
            if (result.result == Results.Success)
                return Ok(result.statistics);
            return NotFound();
        }

        [HttpPost]
        [Route("reporting/statistics01lvl1")]
        public async Task<IActionResult> GetStatistics0101(string xml)
        {
            var seniorcitizen = await _repo.GetStatistics01A01(xml);
            var pwd = await _repo.GetStatistics01A02(xml);
            var indigent = await _repo.GetStatistics01A03(xml);
            var voter = await _repo.GetStatistics01A04(xml);
            var organization = await _repo.GetStatistics01A05(xml);
            var occupation = await _repo.GetStatistics01A06(xml);
          
            return Ok(new { Results.Success
                            ,seniorcitizen = seniorcitizen.statistics
                            , pwd = pwd.statistics
                            , indigent = indigent.statistics
                            ,voter = voter.statistics
                            , organization = organization.statistics
                            , occupation = occupation.statistics
                            });
        }

        [HttpPost]
        [Route("reporting/statistics02lvl1")]
        public async Task<IActionResult> GetStatistics0201(int type1, int type2, string code)
        {
            var seniorcitizen = await _repo.GetStatistics02A01(type1, type2, code);
            var pwd = await _repo.GetStatistics02A02(type1, type2, code);
            var indigent = await _repo.GetStatistics02A03(type1, type2, code);
            var voter = await _repo.GetStatistics02A04(type1, type2, code);
            var organization = await _repo.GetStatistics02A05(type1, type2, code);
            var occupation = await _repo.GetStatistics02A06(type1, type2, code);

            return Ok(new
            {
                Results.Success
                            ,
                seniorcitizen = seniorcitizen.statistics
                            ,
                pwd = pwd.statistics
                            ,
                indigent = indigent.statistics
                            ,
                voter = voter.statistics
                            ,
                organization = organization.statistics
                            ,
                occupation = occupation.statistics
            });
        }

        [HttpPost]
        [Route("reporting/statistics01lvl2")]
        public async Task<IActionResult> GetStatistics0102(string xml)
        {
            var disability = await _repo.GetStatistics01B01(xml);
            var precinct = await _repo.GetStatistics01B02(xml);
            var organization = await _repo.GetStatistics01B03(xml);
            var occupation = await _repo.GetStatistics01B04(xml);

            return Ok(new
            {
                Results.Success
                ,disability = disability.statistics
                ,precinct = precinct.statistics
                ,organization = organization.statistics
                ,occupation = occupation.statistics
            });
        }

        [HttpPost]
        [Route("reporting/statistics02lvl2")]
        public async Task<IActionResult> GetStatistics0202(int type1, int type2, string code)
        {
            var disability = await _repo.GetStatistics02B01(type1, type2, code);
            var precinct = await _repo.GetStatistics02B02(type1, type2, code);
            var organization = await _repo.GetStatistics02B03(type1, type2, code);
            var occupation = await _repo.GetStatistics02B04(type1, type2, code);

            return Ok(new
            {
                Results.Success
                ,
                disability = disability.statistics
                ,
                precinct = precinct.statistics
                ,
                organization = organization.statistics
                ,
                occupation = occupation.statistics
            });
        }

        [HttpPost]
        [Route("reporting/statistics01lvl3")]
        public async Task<IActionResult> GetStatistics0103(string xml)
        {
            var gender = await _repo.GetStatistics01C01(xml);
            var status = await _repo.GetStatistics01C02(xml);
            var agebracket = await _repo.GetStatistics01C03(xml);
            var bloodtype = await _repo.GetStatistics01C04(xml);

            return Ok(new
            {
                Results.Success
                ,gender = gender.statistics
                ,status = status.statistics
                ,agebracket = agebracket.statistics
                ,bloodtype = bloodtype.statistics
            });
        }

        [HttpPost]
        [Route("reporting/statistics02lvl3")]
        public async Task<IActionResult> GetStatistics0203(int type1, int type2, string code)
        {
            var gender = await _repo.GetStatistics02C01(type1, type2, code);
            var status = await _repo.GetStatistics02C02(type1, type2, code);
            var agebracket = await _repo.GetStatistics02C03(type1, type2, code);
            var bloodtype = await _repo.GetStatistics02C04(type1, type2, code);

            return Ok(new
            {
                Results.Success
                ,
                gender = gender.statistics
                ,
                status = status.statistics
                ,
                agebracket = agebracket.statistics
                ,
                bloodtype = bloodtype.statistics
            });
        }

        [HttpPost]
        [Route("reporting/maximizedcontrol01lvl01")]
        public async Task<IActionResult> MaximizedControl01lvl01(string xml, int type)
        {
            var result = await _repo.MaximizedControl01lvl01(xml, type);
            if (result.result == Results.Success)
                return Ok(result.data);
            return NotFound();
        }

        [HttpPost]
        [Route("reporting/maximizedcontrol02lvl01")]
        public async Task<IActionResult> MaximizedControl02lvl01(int loctype, string code, int type)
        {
            var result = await _repo.MaximizedControl02lvl01(loctype, code, type);
            if (result.result == Results.Success)
                return Ok(result.data);
            return NotFound();
        }

        [HttpPost]
        [Route("reporting/maximizedcontrol02lvl03")]
        public async Task<IActionResult> MaximizedControl02lvl03(int loctype, string code, int type)
        {
            var result = await _repo.MaximizedControl02lvl03(loctype, code, type);
            if (result.result == Results.Success)
                return Ok(result.data);
            return NotFound();
        }


        [HttpPost]
        [Route("reporting/maximizedcontrol01lvl03")]
        public async Task<IActionResult> MaximizedControl01lvl03(string xml, int type)
        {
            var result = await _repo.MaximizedControl01lvl03(xml, type);
            if (result.result == Results.Success)
                return Ok(result.data);
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

        [HttpPost]
        [Route("reporting/reportComplaints")]
        public async Task<IActionResult> ShowComplaints(string from, string to)
        {
            var result = await _repo.GetComplaints(from, to);
            if (result.result == Results.Success)
                return Ok(result.household);
            return NotFound();
        }

        [HttpPost]
        [Route("reporting/reportOrgs")]
        public async Task<IActionResult> ShowOrgs(string xml)
        {
            var result = await _repo.GetOrgs(xml);
            if (result.result == Results.Success)
                return Ok(result.household);
            return NotFound();
        }

        [HttpPost]
        [Route("reporting/getreportsettings")]
        public async Task<IActionResult> ShowPreferences([FromBody] ReportSettings settings)
        {
            var result = await _repo.GetPreferences(settings);
            if (result.result == Results.Success)
                return Ok(result.household);
            return NotFound();
        }

        [HttpPost]
        [Route("reporting/addreportsettings")]
        public async Task<IActionResult> ReportPreferenceAdd([FromBody] ReportSettings settings)
        {
            //var result = await _repo.AddPosition(position.JsonString);
            var result = await _repo.AddReportPreference(settings);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
    }
}
