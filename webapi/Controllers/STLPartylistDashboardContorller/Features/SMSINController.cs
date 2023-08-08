using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

using Comm.Commons.Advance;
using Comm.Commons.Extensions;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard;
using webapi.App.Model.User;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.RequestModel.Common;
using webapi.App.RequestModel.Feature;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    public class SMSINController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ISMSRepository _repo;
        public SMSINController(IConfiguration config, ISMSRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("smsin")]
        public async Task<IActionResult> Task0a([FromBody] BIMSSMSIN req)
        {
            var result = await _repo.SMSINAsync(req);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("smsin/notification")]
        public async Task<IActionResult> Task0b([FromBody] FilterRequest req)
        {
            var result = await _repo.SMSINNotification(req);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", totalunreadinbox = result.totalunreadinbox, sender = result.resident });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", totalunreadinbox = result.totalunreadinbox, sender = result.resident });
            return NotFound();
        }
        [HttpPost]
        [Route("chat/isread")]
        public async Task<IActionResult> Task0c([FromBody] MessengerAppRequest req)
        {
            var result = await _repo.ChatMessageReadAsync(req);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
    }
}
