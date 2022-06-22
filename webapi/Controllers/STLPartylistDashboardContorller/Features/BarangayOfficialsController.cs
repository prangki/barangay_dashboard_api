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

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class BarangayOfficialsController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBrangayOfficialsRepository _repo;
        public BarangayOfficialsController(IConfiguration config, IBrangayOfficialsRepository repo)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost]
        [Route("brgyofl")]
        public async Task<IActionResult> Task01([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadMBarangayOfficial(request);
            if (result.result == Results.Success)
                return Ok(result.brgyofl);
            return NotFound();
        }
        [HttpPost]
        [Route("brgyofl/new")]
        public async Task<IActionResult> Task02([FromBody] BarangayOfficial request)
        {
            var repoResult = await _repo.BarangayOfficialAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", BrgyOfl = repoResult.oflid, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }
        [HttpPost]
        [Route("brgyofl/edit")]
        public async Task<IActionResult> Task03([FromBody] BarangayOfficial request)
        {
            var repoResult = await _repo.UpdateBarangayOfficialAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }

    }
}
