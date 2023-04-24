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
    [Route("app/v1/stldashboard/emergencycontactperson")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class EmergencyContactPersonController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IEmergencyContactPersonRepository _repo;
        public EmergencyContactPersonController(IConfiguration config, IEmergencyContactPersonRepository repo)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> Task01([FromBody] FilterRequest request)
        {
            var result = await _repo.Load_EmergencyContactPerson(request);
            if (result.result == Results.Success)
                return Ok(result.emgycp);
            return NotFound();
        }

        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> Task0a([FromBody] Emergency_Contact_Person request)
        {
            var result = await _repo.EmergencyContactPersonAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = request });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Task0b([FromBody] Emergency_Contact_Person request)
        {
            var result = await _repo.EmergencyContactPersonAsync(request, true);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = request });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
    }
}
