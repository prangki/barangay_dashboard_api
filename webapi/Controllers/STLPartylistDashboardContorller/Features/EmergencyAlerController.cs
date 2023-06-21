using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.STLDashboardModel;
using Comm.Commons.Extensions;
using webapi.App.Features.UserFeature;
using Newtonsoft.Json;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;
using System.IO;
using System.Text;
using webapi.App.RequestModel.AppRecruiter;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard/emergencyalert")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class EmergencyAlerController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IEmergencyAlertRepository _repo;

        public EmergencyAlerController(IConfiguration config, IEmergencyAlertRepository repo)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> Task01([FromBody] FilterRequest request)
        {
            var result = await _repo.Load_EmergencyAlert(request);
            if (result.result == Results.Success)
                return Ok(result.emgyalert);
            return NotFound();
        }

        [HttpPost]
        [Route("closed")]
        public async Task<IActionResult> Task0a([FromBody] EmergencyAlert request)
        {
            var result = await _repo.ClosedEmergencyAlertAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = request });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("alert")]
        public async Task<IActionResult> Task0b([FromBody] FilterRequest request)
        {
            var result = await _repo.Load_EmergencyAlertRequest(request);
            var res = await _repo.TotaldEmergencyAlertAsync(request);
            if (result.result == Results.Success)
                return Ok(new {Status = "ok", emergencyalert = result.emgyalert, totalemrgyalert = res.total_alert });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", emergencyalert = result.emgyalert, totalemrgyalert = res.total_alert });
            return NotFound();
        }

        [HttpPost]
        [Route("id")]
        public async Task<IActionResult> Task0C([FromBody] FilterRequest request)
        {
            var result = await _repo.Load_EmergencyAlertID(request);
            if (result.result == Results.Success)
                return Ok(result.emgyalert);
            return NotFound();
        }

        [HttpPost]
        [Route("profile")]
        public async Task<IActionResult> LoadEmergencyPofile(string userId)
        {
            var result = await _repo.LoadEmergencyPofile(userId);
            if (result.result == Results.Success)
                return Ok(result.profile);
            return NotFound();
        }
    }
}
